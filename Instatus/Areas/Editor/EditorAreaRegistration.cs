using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Instatus.Models;
using Instatus.Web;
using Instatus.Widgets;

namespace Instatus.Areas.Editor
{
    public class EditorAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Editor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {                      
            context.MapRouteLowercase(
                "Editor_default",
                "Editor/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional, area = AreaName },
                null,
                new string[] { "Instatus.Areas.Editor.Controllers" }
            );

            Startup.Parts.Add(TagWidget.Script("~/Scripts/scaffold.js", true, WebConstant.Scope.Admin));
            Startup.Parts.Add(TagWidget.Stylesheet("~/Content/scaffold.css", WebConstant.Scope.Admin));
        }
    }

    public class EditorAreaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Instatus.Areas.Editor.Controllers.ArticleController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Editor.Controllers.BrandController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Editor.Controllers.CatalogController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Editor.Controllers.FileController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Editor.Controllers.NewsController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Editor.Controllers.OrganizationController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Editor.Controllers.PostController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Editor.Controllers.ProfileController>().InstancePerDependency();
            builder.RegisterType<Instatus.Areas.Editor.Controllers.TagController>().InstancePerDependency();
        }

        public EditorAreaModule()
        {
            RouteTable.Routes.RegisterArea<EditorAreaRegistration>();
        }
    }
}
