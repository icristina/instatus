using System.Web.Mvc;
using Instatus;
using Instatus.Web;

namespace Instatus.Areas.Auth
{
    public class AuthAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Auth";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRouteLowercase(
                WebConstant.Route.AccountVerification,
                "Auth/Verification/{id}/{token}",
                new { 
                    action = "Verification", 
                    controller = "Account"
                },
                null,
                new string[] { "Instatus.Areas.Auth.Controllers" }
            );            
            
            context.MapRouteLowercase(
                "Auth_default",
                "Auth/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional, area = AreaName },
                null,
                new string[] { "Instatus.Areas.Auth.Controllers" }
            );
        }
    }
}
