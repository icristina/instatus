using System.Web.Mvc;

namespace Instatus.Areas.Moderator
{
    public class ModeratorAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Moderator";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {                      
            context.MapRouteLowercase(
                "Moderator_default",
                "Moderator/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
