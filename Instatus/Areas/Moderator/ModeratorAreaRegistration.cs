using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Routing;

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
                new { action = "Index", id = UrlParameter.Optional, area = AreaName },
                null,
                new string[] { "Instatus.Areas.Moderator.Controllers" }
            );
        }
    }

    public class ModeratorAreaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Instatus.Areas.Moderator.Controllers.ActivityController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Moderator.Controllers.ExportController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Moderator.Controllers.PostController>().InstancePerDependency();
        }

        public ModeratorAreaModule()
        {
            RouteTable.Routes.RegisterArea<ModeratorAreaRegistration>();
        }
    }
}
