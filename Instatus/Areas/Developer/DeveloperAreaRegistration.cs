using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

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

    public class DeveloperAreaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Instatus.Areas.Developer.Controllers.CredentialController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Developer.Controllers.DomainController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Developer.Controllers.LogController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Developer.Controllers.PhraseController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Developer.Controllers.RedirectController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Developer.Controllers.RegionController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Developer.Controllers.TaxonomyController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Developer.Controllers.UserController>().InstancePerDependency();
        }

        public DeveloperAreaModule()
        {
            RouteTable.Routes.RegisterArea<DeveloperAreaRegistration>();
        }
    }
}
