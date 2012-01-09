﻿using System.Web.Mvc;

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

        public const string RootRouteName = "Microsite_Root";
        public const string PageRouteName = "Microsite_Page";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                RootRouteName,
                "",
                new { controller = "Page", action = "Details" }
            );

            context.Routes.MapNavigableRoute(RoutePrefix, areaName: AreaName);

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
