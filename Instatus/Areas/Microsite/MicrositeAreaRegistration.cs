using System.Web.Routing;
using System.Web.Mvc;
using Instatus;
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
            RouteTable.Routes.MapContentPageRoute("page", areaName: AreaName);
            
            context.MapRouteLowercase(
                "Microsite_Default",
                "Microsite/{controller}/{action}/{id}",
                new { controller = "Stream", action = "Index", id = UrlParameter.Optional, area = AreaName },
                null,
                new string[] { "Instatus.Areas.Microsite.Controllers" }
            );
        }
    }
}
