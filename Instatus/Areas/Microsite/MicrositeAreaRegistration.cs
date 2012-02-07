using System.Web.Mvc;
using Instatus.Web;

namespace Instatus.Areas.Microsite
{
    public class MicrositeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Microsite";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHomeRoute(controllerName: "Page", areaName: AreaName, ns: "Instatus.Areas.Microsite.Controllers");
            context.Routes.MapPageRoute(WebRoute.PagePrefix, areaName: AreaName, ns: "Instatus.Areas.Microsite.Controllers");

            context.MapRouteLowercase(
                "Microsite_Default",
                "Microsite/{controller}/{action}/{id}",
                new { controller = "Page", action = "Index", id = UrlParameter.Optional, area = AreaName },
                null,
                new string[] { "Instatus.Areas.Microsite.Controllers" }
            );

            context.MapRoute(
                "Microsite_Robots",
                "robots.txt",
                new { controller = "Robots", action = "Index" },
                null,
                new string[] { "Instatus.Areas.Microsite.Controllers" }
            );
        }
    }
}
