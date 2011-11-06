using System.Web.Mvc;
using Instatus.Web;
using System.Collections.Generic;

namespace Instatus.Areas.Facebook
{
    public class FacebookAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Facebook";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Facebook_channel",
                "channel.html",
                new { controller = "Channel", action = "Index" }
            );            
            
            context.MapRoute(
                "Facebook_default",
                "Facebook/{controller}/{action}/{id}",
                new { controller = "Tab", action = "Index", id = UrlParameter.Optional }
            );

            WebPart.Catalog.Add(new WebPartial()
            {
                Zone = WebZone.Scripts,
                ActionName = "RegisterScripts",
                Parameters = new List<WebParameter>()
                {
                    new WebParameter("area", AreaName),
                    new WebParameter("controller", "facebook")
                }
            });
        }
    }
}
