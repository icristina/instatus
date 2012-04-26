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

            WebCatalog.Parts.Add(TagWidget.Script("~/Scripts/scaffold.js", false, WebConstant.Scope.Admin));
            WebCatalog.Parts.Add(TagWidget.Stylesheet("~/Content/scaffold.css", WebConstant.Scope.Admin));
        }
    }

    public class EditorAreaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Instatus.Areas.Editor.Controllers.ArticleController>().InstancePerDependency();
        }

        public EditorAreaModule()
        {
            RouteTable.Routes.RegisterArea<EditorAreaRegistration>();
        }
    }
}
