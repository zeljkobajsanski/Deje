using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Deje.Core.Model;
using Deje.Core.Repository;

namespace Deje.Web.Admin.Controllers
{
    public class VrsteDobavljacaController : Controller
    {
        private readonly IVrsteDobavljacaRepository m_VrsteDobavljacaRepository;

        private readonly IDelatnostiRepository m_DelatnostiRepository;

        public VrsteDobavljacaController(IVrsteDobavljacaRepository vrsteDobavljacaRepository, IDelatnostiRepository delatnostiRepository)
        {
            m_VrsteDobavljacaRepository = vrsteDobavljacaRepository;
            m_DelatnostiRepository = delatnostiRepository;
        }

        public ActionResult Index()
        {
            var delatnosti = m_DelatnostiRepository.VratiSve();
            ViewData["Delatnosti"] = delatnosti;
            return View(Enumerable.Empty<VrstaDobavljaca>());
        }

        public ActionResult VratiVrsteDobavljacaCallback(int idDelatnosti)
        {
            var vrsteDobavljaca = m_VrsteDobavljacaRepository.VratiZaDelatnost(idDelatnosti);
            return PartialView("VrsteDobavljacaGrid", vrsteDobavljaca);
        }

        public ActionResult Sacuvaj(int idDelatnosti, VrstaDobavljaca vrstaDobavljaca)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Podaci nisu ispravni";
            } else
            {
                vrstaDobavljaca.IdDelatnosti = idDelatnosti;
                m_VrsteDobavljacaRepository.Save(vrstaDobavljaca);
            }
            var vrsteDobavljaca = m_VrsteDobavljacaRepository.VratiZaDelatnost(idDelatnosti);
            return PartialView("VrsteDobavljacaGrid", vrsteDobavljaca);
        }

        public ActionResult Obrisi(int idDelatnosti, int id)
        {
            try
            {
                m_VrsteDobavljacaRepository.Delete(id);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            var vrsteDobavljaca = m_VrsteDobavljacaRepository.VratiZaDelatnost(idDelatnosti);
            return PartialView("VrsteDobavljacaGrid", vrsteDobavljaca);
        }

    }
}
