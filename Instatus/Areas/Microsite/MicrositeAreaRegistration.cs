using System.Web.Mvc;

namespace Instatus.Areas.Microsite
{
    public class MicrositeAreaRegistration : AreaRegistration
    {
        public static string RoutePrefix = "Site";
        
        public override string AreaName
        {
            get
            {
                return "Microsite";
            }
        }

        public const string PageRouteName = "Microsite_Page";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                PageRouteName,
                RoutePrefix + "/{slug}",
                new { controller = "Page", action = "Details", slug = UrlParameter.Optional }
            );

            context.MapRoute(
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
