using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Deje.Core.Manager;
using Deje.Core.Model;
using Deje.Core.Repository;
using Deje.Core.Status;
using Deje.Web.Admin.Model;
using Deje.Web.Admin.Services;
using DevExpress.Web.ASPxUploadControl;
using DevExpress.Web.Mvc;

namespace Deje.Web.Admin.Controllers
{
    public class DobavljaciController : Controller
    {
        private readonly IDobavljaciRepository m_DobavljaciRepository;

        private readonly IDelatnostiRepository m_DelatnostiRepository;

        private readonly DobavljaciManager m_DobavljaciManager;

        private readonly IVrsteDobavljacaRepository m_VrsteDobavljacaRepository;

        private readonly IStatusiRepository m_StatusiRepository;

        public DobavljaciController(IDobavljaciRepository dobavljaciRepository, IDelatnostiRepository delatnostiRepository, DobavljaciManager dobavljaciManager,
            IVrsteDobavljacaRepository vrsteDobavljacaRepository, IStatusiRepository statusiRepository)
        {
            m_DobavljaciRepository = dobavljaciRepository;
            m_DelatnostiRepository = delatnostiRepository;
            m_DobavljaciManager = dobavljaciManager;
            m_VrsteDobavljacaRepository = vrsteDobavljacaRepository;
            m_StatusiRepository = statusiRepository;
        }

        public ActionResult Index()
        {
            var dobavljaci = m_DobavljaciRepository.VratiSve();
            return View(dobavljaci);
        }

        public PartialViewResult VratiDobavljacePartial()
        {
            var dobavljaci = m_DobavljaciRepository.VratiSve();
            return PartialView("DobavljaciGrid", dobavljaci);
        }

        public ActionResult KreirajNovog()
        {
            var dobavljac = new Dobavljac{GpsLatitude = 45.25, GpsLongitude = 19.85, Kontakt = new Kontakt()};
            ViewData["Delatnosti"] = m_DelatnostiRepository.VratiSve();
            ViewBag.StatusiDobavljaca = m_StatusiRepository.VratiStatuseDobavljaca();
            TempData["Status"] = new StatusKreirajNovi();
            return View("PregledDobavljaca", dobavljac);
        }

        public ActionResult Sacuvaj(Dobavljac dobavljac)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Delatnosti"] = m_DelatnostiRepository.VratiSve();
                if (dobavljac.IdDelatnosti.HasValue)
                {
                    var vrsteDobavljaca = m_VrsteDobavljacaRepository.VratiZaDelatnost(dobavljac.IdDelatnosti.Value);
                    ViewBag.VrsteDobavljaca = vrsteDobavljaca;
                }
                ViewBag.StatusiDobavljaca = m_StatusiRepository.VratiStatuseDobavljaca();
                TempData["Status"] = new StatusValidationError();
                return View("PregledDobavljaca", dobavljac);
            }
            
            var slike = UploadControlExtension.GetUploadedFiles("slikaDobavljacaUpload");
            var slika = slike.SingleOrDefault();
            if (slika != null && slika.ContentLength > 0)
            {
                var putanja = ((BlobStorage) HttpContext.Application["storage"]).SacuvajSlikuDobavljaca(slika.FileContent,
                                                                                          slika.ContentType,
                                                                                          Path.GetExtension(slika.FileName));
                dobavljac.Slika = putanja;
            }
            var id = m_DobavljaciRepository.Save(dobavljac);
            TempData["Status"] = new StatusSaved();
            return RedirectToAction("Izmeni", new {id});
        }

        public ActionResult Izmeni(int id)
        {
            var dobavljac = m_DobavljaciRepository.VratiPoId(id);
            ViewData["Delatnosti"] = m_DelatnostiRepository.VratiSve();
            var vrsteDobavljaca = m_VrsteDobavljacaRepository.VratiZaDelatnost(dobavljac.IdDelatnosti.Value);
            ViewBag.VrsteDobavljaca = vrsteDobavljaca;
            ViewBag.StatusiDobavljaca = m_StatusiRepository.VratiStatuseDobavljaca();
            return View("PregledDobavljaca", dobavljac);
        }

        public ActionResult Obrisi(int id)
        {
            m_DobavljaciRepository.Delete(id);
            return RedirectToAction("Index");
        }

        public PartialViewResult VratiRezultatePretrageCallback(string rezultati)
        {
            var jss = new JavaScriptSerializer();
            var rez = jss.Deserialize<RezultatPretrage[]>(rezultati);
            return PartialView("PretragaAdresaGrid", rez);
        }

        public PartialViewResult VratiVrsteDobavljacaCallback(int idDelatnosti)
        {
            var vrsteDobavljaca = m_VrsteDobavljacaRepository.VratiZaDelatnost(idDelatnosti);
            ViewBag.VrsteDobavljaca = vrsteDobavljaca;
            return PartialView("VrsteDobavljacaCombo");
        }

        [HttpPost]
        public void SacuvajLokaciju(int id, double latitude, double longitude, int zoom)
        {
            m_DobavljaciManager.SacuvajLokacijuDobavljaca(id,latitude, longitude, zoom);
        }

        public ActionResult Pretraga()
        {
            return View();
        }

        public JsonResult Pretrazi(double latitude, double longitude, double distance)
        {
            var rezultati = m_DobavljaciRepository.VratiDobavljaceUOkolini(latitude, longitude, distance);
            return Json(rezultati, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VratiGalerijuSlika(int id)
        {
            var slike = m_DobavljaciRepository.VratiSlikeDobavljaca(id).Select(x => new {image = @Url.Content("~/Slike/" + x.SlikaSaEkstenzijom)}).ToArray();
            return Json(slike, JsonRequestBehavior.AllowGet);
        }

        public void DodajUGaleriju(int id)
        {
            var files = UploadControlExtension.GetUploadedFiles("upload");
            foreach (var uploadedFile in files)
            {
                SacuvajSlikuUGaleriju(id, uploadedFile);
            }
        }

        private void SacuvajSlikuUGaleriju(int idDobavljaca, UploadedFile slika)
        {
            var slikaDobavljaca = new SlikeDobavljaca { Id = Guid.NewGuid(), IdDobavljaca = idDobavljaca, Ekstenzija = ".jpg" };
            m_DobavljaciRepository.DodajSliku(slikaDobavljaca);
        }

        public void ObrisiSliku(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var fullPath = Server.MapPath(id);
                try
                {
                    System.IO.File.Delete(fullPath);
                }
                catch
                {
                }
                var lastSlash = id.LastIndexOf("/");
                var image = id.Substring(lastSlash);
                var imageId = Path.GetFileNameWithoutExtension(image);
                m_DobavljaciRepository.ObrisiSliku(Guid.Parse(imageId));
            }
            
        }

        public PartialViewResult VratiDobavljaceComboBoxCallback(int width, int? idDobavljaca)
        {
            var dobavljaci = m_DobavljaciRepository.VratiSve();
            ViewBag.Dobavljaci = dobavljaci;
            ViewBag.ComboWidth = width;
            return PartialView("DobavljaciComboBox", idDobavljaca);
        }
    }
}
