﻿using System.Web.Mvc;
using Instatus.Web;
using System.Collections.Generic;

namespace Instatus.Areas.Google
{
    public class GoogleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Google";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRouteLowercase(
                "Google_default",
                "Google/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional, area = AreaName },
                null,
                new string[] { "Instatus.Areas.Google.Controllers" }
            );

            WebPart.Catalog.Add(new WebPartial()
            {
                Zone = WebZone.Scripts,
                ActionName = "RegisterScripts",
                Parameters = new List<WebParameter>()
                {
                    new WebParameter("area", AreaName),
                    new WebParameter("controller", "google")
                }
            });
        }
    }
}
