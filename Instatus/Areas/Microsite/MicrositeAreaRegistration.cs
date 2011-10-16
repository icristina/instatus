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
        public const string WidgetRouteName = "Microsite_Widget";
        public const string RobotsRouteName = "Microsite_Robots";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                PageRouteName,
                RoutePrefix + "/{slug}",
                new { controller = "Article", action = "Details", slug = UrlParameter.Optional }
            );

            context.MapRoute(
                WidgetRouteName,
                "article/{action}/{id}",
                new { controller = "Article", action = "Widget", id = UrlParameter.Optional }
            );

            context.MapRoute(
                RobotsRouteName,
                "robots.txt",
                new { controller = "Robots", action = "Index" }
            );
        }
    }
}
