﻿using System.Web.Mvc;
using Instatus.Web;
using System.Collections.Generic;

namespace Instatus.Areas.Typekit
{
    public class TypekitAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Typekit";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Typekit_default",
                "Typekit/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            WebPart.Catalog.Add(new WebPartial()
            {
                Zone = WebZone.Head,
                ActionName = "RegisterScripts",
                Parameters = new List<WebParameter>()
                {
                    new WebParameter("area", AreaName),
                    new WebParameter("controller", "typekit")
                }
            });
        }
    }
}
