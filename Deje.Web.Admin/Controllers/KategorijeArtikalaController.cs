using System.Web.Mvc;
using Deje.Core.Model;
using Deje.Core.Repository;

namespace Deje.Web.Admin.Controllers
{
    public class KategorijeArtikalaController : Controller
    {
        private readonly IKategorijeArtikalaRepository m_KategorijeArtikalaRepository;

        public KategorijeArtikalaController(IKategorijeArtikalaRepository kategorijeArtikalaRepository)
        {
            m_KategorijeArtikalaRepository = kategorijeArtikalaRepository;
        }

        public ActionResult Index()
        {
            var kategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
            return View(kategorijeArtikala);
        }

        public PartialViewResult VratiKategorijeArtikalaCallback()
        {
            var kategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
            return PartialView("KategorijeArtiaklaGrid", kategorijeArtikala);
        }

        public PartialViewResult SacuvajKategorijuArtikla(KategorijaArtikla kategorijaArtikla)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Podaci nisu validni";
            } else
            {
                m_KategorijeArtikalaRepository.Sacuvaj(kategorijaArtikla);
            }
            var kategorijeArtikala = m_KategorijeArtikalaRepository.VratiSve();
            return PartialView("KategorijeArtiaklaGrid", kategorijeArtikala);
        }

        public PartialViewResult ObrišiKategorijuArtikla(int id)
        {
            m_KategorijeArtikalaRepository.Obrisi(id);
            return VratiKategorijeArtikalaCallback();
        }
    }
}