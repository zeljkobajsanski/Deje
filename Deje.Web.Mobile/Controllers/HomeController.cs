using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using System.Xml.Linq;
using Deje.Core.Model;
using Deje.Core.Repository;
using Deje.Lucene;
using Deje.Web.Mobile.Models;

namespace Deje.Web.Mobile.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDelatnostiRepository m_DelatnostiRepository;

        private readonly IKategorijeArtikalaRepository m_KategorijeArtikalaRepository;

        private readonly IArtikliRepository m_ArtikliRepository;

        private readonly IDobavljaciRepository m_DobavljaciRepository;

        public HomeController(IDelatnostiRepository delatnostiRepository, IKategorijeArtikalaRepository kategorijeArtikalaRepository, 
            IArtikliRepository artikliRepository, IDobavljaciRepository dobavljaciRepository)
        {
            m_DelatnostiRepository = delatnostiRepository;
            m_KategorijeArtikalaRepository = kategorijeArtikalaRepository;
            m_ArtikliRepository = artikliRepository;
            m_DobavljaciRepository = dobavljaciRepository;
        }

        public ActionResult Index()
        {
            return View("~/index.cshtml");
        }

        public JsonResult UcitajDelatnosti()
        {
            //Thread.Sleep(2000);
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var delatnosti = m_DelatnostiRepository.VratiSve().OrderBy(x => x.Naziv).ToList();
            if (delatnosti.Count > 1)
            {
                delatnosti.Insert(0, new Delatnost{Id = 0, Naziv = "Sve delatnosti"});
            }
            return Json(delatnosti, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Pronadji(double latituda, double longituda, int udaljenost, int delatnost)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var kategorijeArtikala = m_KategorijeArtikalaRepository.VratiKategorijeArtikalaUOkolini(latituda, longituda, udaljenost, delatnost).OrderBy(x => x.Naziv);
            var grupeArtikala = new
            {
                count = kategorijeArtikala.Count(),
                grupeArtikala = kategorijeArtikala.Select(x => new { id = x.Id, naziv = x.Naziv, count = x.BrojArtikala })
            };
            return Json(grupeArtikala, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PronadjiAdresu(string adresa)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            const string serviceIp = "http://dev.virtualearth.net/REST/v1/Locations";
            using (var client = new WebClient())
            {
                client.QueryString.Add("key", "AoAspjo-51Af3fRuiPmgnwZ6969gydoWrZ26rmvfE9tk10e7Q5JYNvs4rXLQ8VKn");
                client.QueryString.Add("query", adresa);
                client.QueryString.Add("o", "xml");
                var response = client.DownloadString(serviceIp);
                var result = XDocument.Parse(response);
                var desc = result.Descendants();
                var nadjeno = new List<LatLng>();
                var statusCode = desc.Single(x => x.Name.LocalName == "StatusCode").Value;
                if ("200" == statusCode)
                {
                    var locations = desc.Where(x => x.Name.LocalName == "Location");
                    
                    foreach (var location in locations)
                    {
                        var locationDesc = location.Descendants();
                        var point = locationDesc.Single(x => x.Name.LocalName == "Point");
                        nadjeno.Add(new LatLng()
                        {
                            Naziv = locationDesc.Single(x => x.Name.LocalName == "FormattedAddress").Value,
                            Latitude = Decimal.Parse(point.Descendants().Single(x => x.Name.LocalName == "Latitude").Value),
                            Longitude = Decimal.Parse(point.Descendants().Single(x => x.Name.LocalName == "Longitude").Value),
                        });
                    }
                } 
                return Json(new {status = statusCode, count = nadjeno.Count, result = nadjeno}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PretraziArtikle(int idGrupeArtikala, double latituda, double longituda, int razdaljina)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var artikli = m_ArtikliRepository.NadjiArtikleUOkolini(idGrupeArtikala, latituda, longituda, razdaljina);
            var sinonimi = m_ArtikliRepository.NadjiSinonimeUOkolini(idGrupeArtikala, latituda, longituda, razdaljina);
            var data = artikli.Select(x => new
            {
                x.Id,
                x.Naziv,
                Slika = x.Slika,
                Opis = x.Opis ?? string.Empty,
                Tip = "a",
                Broj = 1
            }).ToList();
            data.AddRange(sinonimi.Select(x => new
            {
                x.Id,
                x.Naziv,
                Slika = x.Slika,
                Opis = x.Opis ?? string.Empty,
                Tip = "s",
                Broj = x.BrojArtikala
            }));
            return Json(data.OrderBy(x => x.Naziv), JsonRequestBehavior.AllowGet);
        }

        public JsonResult PretraziArtiklePoNazivu(double latituda, double longituda, int razdaljina, string naziv)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var artikli = m_ArtikliRepository.NadjiArtikleUOkolini(latituda, longituda, razdaljina);
            var index = (Index) HttpContext.Application["Index"];
            var artikliIzIndeksa = index.PretraziArtikle(naziv, true);
            var sinonimi = m_ArtikliRepository.NadjiSinonimeUOkolini(latituda, longituda, razdaljina);
            var sinonimiIzIndeksa = index.PretraziSinonime(naziv, true);

            var qArtikli = from a1 in artikli
                    join a2 in artikliIzIndeksa on a1.Id equals a2.Id
                    orderby a2.Score descending 
                    select new
                    {
                        a1.Id,
                        a1.Naziv,
                        a1.Slika,
                        Opis = a1.Opis ?? string.Empty,
                        Broj = 1,
                        Tip = "a"
                    };
            var qSinonimi = from s1 in sinonimi
                            join s2 in sinonimiIzIndeksa on s1.Id equals s2.Id
                            orderby s2.Score descending 
                            select new
                            {
                                s1.Id,
                                s1.Naziv,
                                s1.Slika,
                                Opis = s1.Opis ?? string.Empty,
                                Broj = s1.BrojArtikala,
                                Tip = "s"
                            };

            var result = qSinonimi.Union(qArtikli).ToArray();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PretraziDobavljace(double latitude, double longitude, int distance, int idArtikla, string tip)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            if (tip == "a")
            {
                var dobavljaci = m_DobavljaciRepository.VratiDobavljaceUOkolini(latitude, longitude, distance, idArtikla)
                .Select(x => new
                {
                    Id = x.Id,
                    Latitude = x.GpsLatitude,
                    Longitude = x.GpsLongitude,
                    x.Naziv,
                    x.Opis,
                    Vrsta = x.VrstaDobavljaca.Naziv,
                    UdaljenostM = x.Udaljenost.Value,
                    Udaljenost = x.Udaljenost.Value.ToString("n0")
                });
                return Json(dobavljaci, JsonRequestBehavior.AllowGet);
            }
            if (tip == "s")
            {
                var dobavljaci = m_DobavljaciRepository.VratiDobavljaceUOkoliniSaSinonimom(latitude, longitude, distance, idArtikla)
                    .Select(x => new
                                     {
                                         Id = x.Id,
                                         Latitude = x.GpsLatitude,
                                         Longitude = x.GpsLongitude,
                                         x.Naziv,
                                         x.Opis,
                                         Vrsta = x.VrstaDobavljaca.Naziv,
                                         UdaljenostM = x.Udaljenost.Value,
                                         Udaljenost = x.Udaljenost.Value.ToString("n0")
                                     });
                dobavljaci = dobavljaci.OrderBy(x => x.UdaljenostM);
                return Json(dobavljaci, JsonRequestBehavior.AllowGet);
            }
            throw new ApplicationException("Tip nije poznat");
        }

        public JsonResult VratiDobavljaca(int id)
        {
            //Thread.Sleep(2000);
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var dobavljac = m_DobavljaciRepository.VratiDobavljacaSaPonudomIKontaktPodacima(id);
            var slika = dobavljac.Slika ?? "Content/pictures_2.png";
            var ponuda = dobavljac.Ponuda.OrderBy(x => x.KategorijaArtikla.Naziv).ToArray();
            
            var ponudaPoKategorijama = ponuda.GroupBy(x => x.KategorijaArtikla.Naziv).Select(x => new {
                Kategorija = x.Key, 
                Artikli = x.Select(a => new {
                    a.Naziv, 
                    Opis = a.Opis ?? string.Empty, 
                    Cena = a.Cena.ToString("n2") + " din", 
                    Slika = a.Slika})}).ToArray();
            var jsonData =
                new
                    {
                        dobavljac.Id,
                        dobavljac.Naziv,
                        VrstaDobavljaca = dobavljac.VrstaDobavljaca.Naziv,
                        dobavljac.Opis,
                        Slika = slika,
                        Ponuda = ponudaPoKategorijama.OrderBy(x => x.Kategorija),
                        Mesto = dobavljac.Kontakt.Adresa + ", " + dobavljac.Kontakt.Mesto,
                        Telefon = !string.IsNullOrEmpty(dobavljac.Kontakt.MobilniTelefon) ? dobavljac.Kontakt.MobilniTelefon : dobavljac.Kontakt.FiksniTelefon,
                        Www = dobavljac.Kontakt.Www,
                        Latitude = dobavljac.GpsLatitude,
                        Longitude = dobavljac.GpsLongitude
                    };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
