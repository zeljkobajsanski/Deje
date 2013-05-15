using System.Web.Mvc;
using System.Web.Security;
using Deje.Core.Status;

namespace Deje.CustomerWebApp.Controllers
{
    public class MyController : Controller
    {
        protected void Status(StatusMessage status)
        {
            TempData["Status"] = status;
        } 

        protected int? IdDobavljaca
        {
            get
            {
                var httpCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (httpCookie != null)
                {
                    var cv = httpCookie.Value;
                    var ticket = FormsAuthentication.Decrypt(cv);
                    if (ticket != null)
                    {
                        var id = ticket.UserData;
                        return int.Parse(id);
                    }
                }
                return null;
            }
        }
    }
}