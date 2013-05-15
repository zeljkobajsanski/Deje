using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Deje.Core.Model;
using Deje.Core.Repository;
using Deje.Core.Status;
using Deje.Web.Admin.Services;
using DevExpress.Web.Mvc;

namespace Deje.Web.Admin.Controllers
{
    public class ArtikliController : Controller
    {

        private readonly IArtikliRepository m_ArtikliRepository;

        private readonly IKategorijeArtikalaRepository m_KategorijeArtikalaRepository;

        private IDobavljaciRepository m_DobavljaciRepository;

        public ArtikliController(IArtikliRepository artikliRepository, IKategorijeArtikalaRepository kategorijeArtikalaRepository, 
            IDobavljaciRepository dobavljaciRepository)
        {
            m_ArtikliRepository = artikliRepository;
            m_KategorijeArtikalaRepository = kategorijeArtikalaRepository;
            m_DobavljaciRepository = dobavljaciRepository;
        } 

        public ActionResult Index(int? idDobavljaca)
        {
            IEnumerable<Artikal> artikli = Enumerable.Empty<Artikal>();
            if (idDobavljaca.HasValue)
            {
                ViewBag.KategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
                artikli = VratiArtikleDobavljaca(idDobavljaca.Value);
                ViewBag.IdDobavljaca = idDobavljaca.Value;
            }
            return View(artikli);
        }

        public PartialViewResult VratiArtikleCallback(int idDobavljaca)
        {
            var kategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
            var artikli = VratiArtikleDobavljaca(idDobavljaca);
            ViewBag.KategorijeArtikala = kategorijeArtikala;
            ViewBag.IdDobavljaca = idDobavljaca;
            return PartialView("ArtikliGrid", artikli);
        }

        public ActionResult Kreiraj(int idDobavljaca)
        {
            ViewBag.KategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
            var dobavljac = m_DobavljaciRepository.VratiPoId(idDobavljaca);
            TempData["Status"] = new StatusKreirajNovi();
            return View("Artikal", new Artikal{IdDobavljaca = idDobavljaca, Aktivan = true, Dobavljac = dobavljac});
        }

        public ActionResult Modifikuj(int id)
        {
            ViewBag.KategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
            var artikal = m_ArtikliRepository.VratiArtikal(id, x => x.Dobavljac);
            return View("Artikal", artikal);
        }

        public ActionResult SacuvajArtikal(Artikal artikal, string nazivDobavljaca)
        {
            ViewBag.KategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
            if (!ModelState.IsValid)
            {
                TempData["Status"] = new StatusValidationError();
                artikal.Dobavljac = new Dobavljac {Naziv = nazivDobavljaca};
                return View("Artikal", artikal);
            }
            var slika = UploadControlExtension.GetUploadedFiles("slikaUploadControl").SingleOrDefault();
            if (slika != null && slika.ContentLength > 0)
            {
                var url = ((BlobStorage) HttpContext.Application["storage"]).SacuvajSlikuArtikla(slika.FileContent, 
                    slika.ContentType, Path.GetExtension(slika.FileName));
                artikal.Slika = url;
            }
            m_ArtikliRepository.Sacuvaj(artikal);
            TempData["Status"] = new StatusSaved();
            return RedirectToAction("Modifikuj", new {artikal.Id});
        }

        private IEnumerable<Artikal> VratiArtikleDobavljaca(int idDobavljaca)
        {
            var artikli = m_ArtikliRepository.VratiArtikleDobaljvaca(idDobavljaca);
            return artikli;
        }



        public PartialViewResult VratiArtiklePretrageCallback(int idDobavljaca)
        {
            ViewBag.IdDobavljaca = idDobavljaca;
            ViewBag.KategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
            var artikli = m_ArtikliRepository.VratiSveArtikle();
            return PartialView("PretragaArtikalaGrid", artikli);
        }

        public ActionResult KopirajArtikal(int idArtikla, int idDobavljaca)
        {
            ViewBag.KategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
            var artikal = m_ArtikliRepository.VratiArtikal(idArtikla, x => x.Dobavljac);
            artikal.Id = 0;
            artikal.IdDobavljaca = idDobavljaca;
            return View("Artikal", artikal);
        }
    }
}
