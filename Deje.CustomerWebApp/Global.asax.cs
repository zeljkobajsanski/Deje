using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Deje.Repository.EF;

namespace Deje.CustomerWebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");
            
            routes.MapRoute(
                "UnosArtikla",
                "Artikli/New",
                new { controller = "Artikli", action = "New" }
            );
            routes.MapRoute(
                "IzmenaArtikla",
                "Artikli/{id}",
                new { controller = "Artikli", action = "Edit" }
            );

            //routes.MapRoute(
            //    "IzmenaDobavljaca",
            //    "Dobavljaci/{id}",
            //    new { controller = "Dobavljaci", action = "Edit" }
            //);
            
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Login", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
            

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
			
			ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ModelContext>());
            Database.SetInitializer<ModelContext>(null);
        }
    }
}