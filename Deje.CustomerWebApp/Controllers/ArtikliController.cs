using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Deje.AzureBlobStorage;
using Deje.Core.Model;
using Deje.Core.Status;
using Deje.CustomerWebApp.Settings;
using Deje.Repository.EF.Repository;
using DevExpress.Web.ASPxUploadControl;
using DevExpress.Web.Mvc;

namespace Deje.CustomerWebApp.Controllers
{
    [Authorize]
    public class ArtikliController : MyController
    {
        private readonly ArtikliRepository m_ArtikliRepository = new ArtikliRepository();

        private readonly KategorijeArtikalaRepository m_KategorijeArtikalaRepository = new KategorijeArtikalaRepository();

        private readonly PictureStorageService m_PictureStorageService = new PictureStorageService();

        public ActionResult Index()
        {
            IEnumerable<Artikal> artikli = Enumerable.Empty<Artikal>();
            artikli = m_ArtikliRepository.VratiArtikleDobaljvaca(IdDobavljaca.Value);
            ViewBag.KategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
            if (Request.IsAjaxRequest())
            {
                return PartialView("PartialArtikliGrid", artikli);
            }
            
            return View(artikli);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var idDobavljaca = IdDobavljaca.Value;
            var artikal = m_ArtikliRepository.VratiArtikal(id);
            if (artikal == null) return RedirectToAction("Index");
            if (artikal.IdDobavljaca != idDobavljaca)
            {
                return RedirectToAction("Index");
            }
            ViewBag.UploadSettings = new ImageValidationSettings();
            ViewBag.KategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
            return View(artikal);
        }

        [HttpPost]
        public ActionResult Edit(Artikal artikal)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.KategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
                ViewBag.UploadSettings = new ImageValidationSettings();
                return View("Edit", artikal);
            }
            var slika = UploadControlExtension.GetUploadedFiles("SlikaUpload", new ImageValidationSettings()).Single();
            bool slikaOk = slika.IsValid;
            if (slika.IsValid && slika.ContentLength > 0)
            {
                try
                {
                    artikal.Slika = m_PictureStorageService.SacuvajSlikuArtikla(slika.FileContent, slika.ContentType,
                                                                       Path.GetExtension(slika.FileName));
                }
                catch (Exception)
                {
                    slikaOk = false;
                }
            }
            try
            {
                m_ArtikliRepository.Sacuvaj(artikal);
                Status(slikaOk
                           ? new StatusSaved()
                           : new StatusMessage(StatusType.Warning, "Podaci su uspešno sačuvani, međutim slika artikla nije sačuvana", null));
            }
            catch (Exception exc)
            {
                Status(new StatusError());
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult New()
        {
            var id = IdDobavljaca.Value;
            var artikal = new Artikal {IdDobavljaca = id};
            ViewBag.UploadSettings = new ValidationSettings
                                         {
                                             AllowedFileExtensions = new[] {".jpeg"}
                                         };
            ViewBag.KategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
            return View(artikal);
        }

        [HttpPost]
        public ActionResult New(Artikal artikal)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.KategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
                ViewBag.UploadSettings = new ImageValidationSettings();
                return View(artikal);
            }
            var slika = UploadControlExtension.GetUploadedFiles("SlikaUpload", new ImageValidationSettings()).Single();
            bool slikaOk = slika.IsValid;
            if (slika.IsValid && slika.ContentLength > 0)
            {
                try
                {
                    artikal.Slika = m_PictureStorageService.SacuvajSlikuArtikla(slika.FileContent, slika.ContentType,
                                                                       Path.GetExtension(slika.FileName));
                }
                catch (Exception)
                {
                    slikaOk = false;
                }
            }
            try
            {
                m_ArtikliRepository.Sacuvaj(artikal);
                Status(slikaOk
                           ? new StatusSaved()
                           : new StatusMessage(StatusType.Warning, "Podaci su uspešno sačuvani, međutim slika artikla nije sačuvana", null));
            }
            catch (Exception exc)
            {
                Status(new StatusError());
            }
            return RedirectToAction("Index");
        }

    }
}
