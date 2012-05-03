using System.Web.Routing;
using System.Web.Mvc;
using Instatus;
using Instatus.Web;
using Autofac;

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
            context.MapRouteLowercase(
                "Microsite_Default",
                "Microsite/{controller}/{action}/{id}",
                new { controller = "Stream", action = "Index", id = UrlParameter.Optional, area = AreaName },
                null,
                new string[] { "Instatus.Areas.Microsite.Controllers" }
            );
        }
    }

    public class MicrositeAreaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Instatus.Areas.Microsite.Controllers.PageController>().InstancePerDependency();
        }
        
        public MicrositeAreaModule(string routePrefix = "page")
        {
            RouteTable.Routes.MapContentPageRoute(routePrefix, areaName: "Microsite", ns: "Instatus.Areas.Microsite.Controllers");
            RouteTable.Routes.RegisterArea<MicrositeAreaRegistration>();
        }
    }
}
