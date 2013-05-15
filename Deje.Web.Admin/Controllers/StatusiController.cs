using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Deje.Core.Model;
using Deje.Repository.EF.Repository;

namespace Deje.Web.Admin.Controllers
{
    public class StatusiController : Controller
    {
        private readonly StatusiRepository m_StatusiRepository;

        public StatusiController(StatusiRepository statusiRepository)
        {
            m_StatusiRepository = statusiRepository;
        }

        public ActionResult Dobavljaci()
        {
            var statusiDobavljaca = m_StatusiRepository.VratiStatuseDobavljaca();
            return View(statusiDobavljaca);
        }

        public PartialViewResult SacuvajStatusDobavljaca(StatusDobavljaca statusDobavljaca)
        {
            if (ModelState.IsValid)
            {
                m_StatusiRepository.Save(statusDobavljaca);
            }
            return VratiStatuseDobavljacaCallback();
        }

        public PartialViewResult VratiStatuseDobavljacaCallback()
        {
            var statusiDobavljaca = m_StatusiRepository.VratiStatuseDobavljaca();
            return PartialView("StatusiDobavljacaGrid", statusiDobavljaca);
        }

    }
}
