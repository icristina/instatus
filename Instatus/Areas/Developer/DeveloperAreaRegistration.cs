using System.Web.Mvc;

namespace Instatus.Areas.Developer
{
    public class DeveloperAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Developer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {                      
            context.MapRouteLowercase(
                "Developer_default",
                "Developer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional, area = AreaName },
                null,
                new string[] { "Instatus.Areas.Developer.Controllers" }
            );
        }
    }
}
