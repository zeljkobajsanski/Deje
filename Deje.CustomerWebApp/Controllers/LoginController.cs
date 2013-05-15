using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Deje.Core.Model;
using Deje.Core.Repository;
using Deje.Core.Status;
using Deje.CustomerWebApp.Services;
using Deje.CustomerWebApp.Utils;
using Deje.Repository.EF.Repository;

namespace Deje.CustomerWebApp.Controllers
{
    public class LoginController : MyController
    {
        private readonly IKorisnickiNaloziRepository m_KorisnickiNaloziRepository = new KorisnickiNaloziRepository();

        private MailService m_MailService = new MailService();

        [HttpGet]
        public ActionResult Index()
        {
            //var korisnickiNalog = new KorisnickiNalog
            //{
            //    KorisnickoIme = "korleone",
            //    Lozinka = HashFunction.ComputeHash("1"),
            //    IdDobavljaca = 1,
            //    EMail = "zeljko.bajsanski@gmail.com"
            //};
            //m_KorisnickiNaloziRepository.Save(korisnickiNalog);
            return View();
        }

        [HttpPost]
        public ActionResult Index(string korisnickoIme, string lozinka, string returnUrl)
        {
            if (String.IsNullOrEmpty(korisnickoIme))
            {
                Status(new StatusMessage(StatusType.Error, "Korisničko ime nije uneto", null));
                return View();
            }
            if (String.IsNullOrEmpty(lozinka))
            {
                Status(new StatusMessage(StatusType.Error, "Lozinka nije uneta", null));
                return View();
            }
            var korisnik = m_KorisnickiNaloziRepository.PostojiKorisnik(korisnickoIme, HashFunction.ComputeHash(lozinka));
            if (korisnik == null)
            {
                Status(new StatusMessage(StatusType.Error, "Proverite korisničko ime i lozinku", null));
                return View();
            }

            bool zapamtiMe = Request.Form["zapamti"] != null;
            var cookie = FormsAuthentication.GetAuthCookie(korisnickoIme, zapamtiMe);
            cookie.Value = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, FormsAuthentication.FormsCookieName,
                                                                          DateTime.Now, DateTime.Now.AddMonths(1),
                                                                          zapamtiMe, korisnik.IdDobavljaca.ToString()));
            Response.Cookies.Add(cookie);
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Edit", "Dobavljaci");
        }

        [HttpGet]
        public ActionResult ZaboravljenaLozinka()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ZaboravljenaLozinka(string email)
        {
            m_MailService.SendMail(email, "Zaboravljena lozinka", "Vaša nova lozinka je");
            return RedirectToAction("Index");
        }

    }
}
