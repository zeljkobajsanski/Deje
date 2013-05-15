using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Deje.Core.Model;
using Deje.Core.Repository;

namespace Deje.Web.Admin.Controllers
{
    public class DelatnostiController : Controller
    {
        private readonly IDelatnostiRepository m_DelatnostiRepository;

        public DelatnostiController(IDelatnostiRepository delatnostiRepository)
        {
            m_DelatnostiRepository = delatnostiRepository;
        }

        public ActionResult Index()
        {
            var delatnosti = m_DelatnostiRepository.VratiSve();
            return View(delatnosti);
        }

        public PartialViewResult VratiDelatnostiPartial()
        {
            var delatnosti = m_DelatnostiRepository.VratiSve();
            return PartialView("DelatnostiGrid", delatnosti);
        }

        public PartialViewResult SacuvajDelatnost(Delatnost delatnost)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "Podaci nisu validni";
            } else
            {
                m_DelatnostiRepository.Save(delatnost);
            }
            return VratiDelatnostiPartial();
        }

        public PartialViewResult Obrisi(int id)
        {
            try
            {
                m_DelatnostiRepository.Delete(id);
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
            }
            return VratiDelatnostiPartial();
        }

    }
}
