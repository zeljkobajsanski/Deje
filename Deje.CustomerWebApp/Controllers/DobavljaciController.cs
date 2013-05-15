using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Deje.AzureBlobStorage;
using Deje.Core.Model;
using Deje.Core.Status;
using Deje.CustomerWebApp.Settings;
using Deje.Repository.EF.Repository;
using DevExpress.Web.Mvc;

namespace Deje.CustomerWebApp.Controllers
{
    [Authorize]
    public class DobavljaciController : MyController
    {
        private readonly DobavljaciRepository m_DobavljaciRepository = new DobavljaciRepository();

        private readonly VrsteDobavljacaRepository m_VrsteDobavljacaRepository = new VrsteDobavljacaRepository();

        private readonly DelatnostiRepository m_DelatnostiRepository = new DelatnostiRepository();

        private PictureStorageService m_PictureStorageService = new PictureStorageService();

        private readonly StatusiRepository m_StatusiRepository = new StatusiRepository();

        [HttpGet]
        public ActionResult Edit()
        {
            
            var dobavljac = m_DobavljaciRepository.VratiPoId(IdDobavljaca.Value);
            ViewBag.Delatnosti = m_DelatnostiRepository.VratiSve();
            ViewBag.VrsteDobavljaca = m_VrsteDobavljacaRepository.VratiZaDelatnost(dobavljac.IdDelatnosti.Value);
            ViewBag.Statusi = m_StatusiRepository.VratiStatuseDobavljaca();
            return View(dobavljac);
        }

        [HttpPost]
        public ActionResult Edit(Dobavljac dobavljac)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Delatnosti = m_DelatnostiRepository.VratiSve();
                ViewBag.VrsteDobavljaca = m_VrsteDobavljacaRepository.VratiZaDelatnost(dobavljac.IdDelatnosti.Value);
                ViewBag.Statusi = m_StatusiRepository.VratiStatuseDobavljaca();
                Status(new StatusValidationError());
                return View(dobavljac);
            }
            var slika = UploadControlExtension.GetUploadedFiles("SlikaUpload", new ImageValidationSettings()).Single();
            bool slikaOk = slika.IsValid;
            if (slika.IsValid && slika.ContentLength > 0)
            {
                try
                {
                    dobavljac.Slika = m_PictureStorageService.SacuvajSlikuDobavljaca(slika.FileContent, slika.ContentType,
                                                                       Path.GetExtension(slika.FileName));
                }
                catch (Exception)
                {
                    slikaOk = false;
                }
            }
            try
            {
                m_DobavljaciRepository.Save(dobavljac);
                Status(slikaOk ? new StatusSaved() : new StatusMessage(StatusType.Warning, "Podaci su uspešno snimljeni, međutim slika nije", null));
            }
            catch (Exception exc)
            {
                ViewBag.Delatnosti = m_DelatnostiRepository.VratiSve();
                ViewBag.VrsteDobavljaca = m_VrsteDobavljacaRepository.VratiZaDelatnost(dobavljac.IdDelatnosti.Value);
                ViewBag.Statusi = m_StatusiRepository.VratiStatuseDobavljaca();
                Status(new StatusError());
                return View(dobavljac);
            }
            return RedirectToAction("Edit");
        }

        public PartialViewResult VrsteDobavljacaPartial(int id)
        {
            var vrste = m_VrsteDobavljacaRepository.VratiZaDelatnost(id);
            ViewBag.VrsteDobavljaca = vrste;
            return PartialView("PartialComboBoxVrsteDobavljaca");
        }

    }
}
