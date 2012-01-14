using System.Web.Mvc;
using Instatus;

namespace Instatus.Areas.Auth
{
    public class AuthAreaRegistration : AreaRegistration
    {
        public const string VerificationRouteName = "VerificationRouteName";
        
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
                VerificationRouteName,
                "Auth/Verification/{id}/{token}",
                new { 
                    action = "Index", 
                    controller = "Verification"
                }
            );            
            
            context.MapRouteLowercase(
                "Auth_default",
                "Auth/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
