using System.Web.Mvc;
using Instatus;
using Instatus.Web;
using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Routing;

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
                WebConstant.Route.ChangePassword,
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

    public class AuthAreaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {            
            builder.RegisterType<Instatus.Areas.Auth.Controllers.AccountController>().InstancePerDependency();
        }

        public AuthAreaModule()
        {
            RouteTable.Routes.RegisterArea<AuthAreaRegistration>();
        }
    }
}
