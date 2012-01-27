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
            context.Routes.MapHomeRoute(controllerName: "Page", areaName: AreaName);
            context.Routes.MapPageRoute(WebRoute.PagePrefix, areaName: AreaName);

            context.MapRouteLowercase(
                "Microsite_Default",
                "Microsite/{controller}/{action}/{id}",
                new { controller = "Page", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Microsite_Robots",
                "robots.txt",
                new { controller = "Robots", action = "Index" }
            );
        }
    }
}
