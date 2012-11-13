using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Instatus.Sample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            LocalizationConfig.RegisterPhrases();
            DbConfig.RegisterProviders();
            AutofacConfig.RegisterContainer();
            MvcConfig.RegisterDisplayModes();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}