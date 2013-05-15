using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Deje.Core.Model;
using Deje.Core.Repository;
using Deje.Web.Admin.Services;
using DevExpress.Web.Mvc;

namespace Deje.Web.Admin.Controllers
{
    public class SinonimiController : Controller
    {
        private readonly IArtikliRepository m_ArtikliRepository;

        public SinonimiController(IArtikliRepository artikliRepository)
        {
            m_ArtikliRepository = artikliRepository;
        }

        public ActionResult Index()
        {
            var sinonimi = m_ArtikliRepository.VratiSinonime();
            return View(sinonimi);
        }

        public ActionResult Artikli()
        {
            var artikliBezSinonima = m_ArtikliRepository.VratiArtikleBezSinonima();
            var sinonimi = m_ArtikliRepository.VratiSinonime();
            ViewData["ArtikliBezSinonima"] = artikliBezSinonima;
            ViewData["Sinonimi"] = sinonimi;
            ViewData["IdSinonima"] = 16;
            return View();
        }

        public ActionResult VratiSinonimeCallback()
        {
            var sinonimi = m_ArtikliRepository.VratiSinonime();
            return PartialView("SinonimiGrid",sinonimi);
        }

        public ActionResult SacuvajSinonim(Sinonim sinonim)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Sinonim"] = sinonim;
            } else
            {
                m_ArtikliRepository.SacuvajSinonim(sinonim);
            }
            return VratiSinonimeCallback();
        }

        public ActionResult SacuvajSliku(int? id)
        {
            var slika = UploadControlExtension.GetUploadedFiles("slikaUpload").SingleOrDefault();
            if (!id.HasValue || slika == null) return null;
            if (slika.ContentLength == 0) return null;
            var path = ((BlobStorage) HttpContext.Application["storage"]).SacuvajSlikuArtikla(slika.FileContent, slika.ContentType,
                                                                                   Path.GetExtension(slika.FileName));
            m_ArtikliRepository.DodeliSlikuSinonimu(id.Value, path);
            
            return null;
        }

        public PartialViewResult VratiArtikleBezSinonimaCallback()
        {
            var artikliBezSinonima = m_ArtikliRepository.VratiArtikleBezSinonima();
            return PartialView("ArtikliGrid", artikliBezSinonima);
        }

        public PartialViewResult VratiArtikleSinonimaCallback(int idSinonima)
        {
            var artikliSinonima = m_ArtikliRepository.VratiArtikleSinonima(idSinonima);
            ViewData["IdSinonima"] = idSinonima;
            ViewData["ArtikliBezSinonima"] = m_ArtikliRepository.VratiArtikleBezSinonima();
            return PartialView("ArtikliSinonima", artikliSinonima);
        }

        public PartialViewResult DodajArtikalSinonimu(int idSinonima, Artikal artikal)
        {
            artikal = m_ArtikliRepository.VratiArtikal(artikal.Id);
            artikal.IdSinonima = idSinonima;
            m_ArtikliRepository.Sacuvaj(artikal);
            return VratiArtikleSinonimaCallback(idSinonima);
        }

        public PartialViewResult ObrisiArtikalSinonima(int id, int idSinonima)
        {
            var artikal = m_ArtikliRepository.VratiArtikal(id);
            artikal.IdSinonima = null;
            m_ArtikliRepository.Sacuvaj(artikal);
            return VratiArtikleSinonimaCallback(idSinonima);
        }
    }
}
