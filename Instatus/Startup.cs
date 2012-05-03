using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Instatus.Areas.Auth;
using Instatus.Areas.Editor;
using Instatus.Areas.Facebook;
using Instatus.Areas.Google;
using Instatus.Entities;
using Instatus.Models;
using Instatus.Services;
using Instatus.Web;
using Instatus.Widgets;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Instatus.Startup), "PreApplicationStart")]
[assembly: WebActivator.PostApplicationStartMethod(typeof(Instatus.Startup), "PostApplicationStart")]

namespace Instatus
{
    public static class Startup
    {
        public static List<Module> Modules = new List<Module>();
        public static Dictionary<ImageSize, Transform> ImageSizes = new Dictionary<ImageSize, Transform>();
        public static IList<Part> Parts = new List<Part>();
        public static string LogOnUrl = "Auth/Account/LogOn";

        private static bool AutoStartup
        {
            get 
            {
                return ConfigurationManager.AppSettings[WebConstant.AppSetting.Bootstrap].AsBoolean(true);
            }
        }
        
        private static void PreApplicationStart()
        {
            if (!AutoStartup)
                return;

            MvcConfiguration();
            IgnoreRoutes();
            Auth();
            ErrorHandling();
            ViewLocation();
            RemoveServerFingerprint();
            Rewriting();
        }

        private static void PostApplicationStart()
        {
            if (!AutoStartup)
                return;
            
            AutofacDependencyResolver();
            DefaultRoute();
            Bundles();
        }

        public static void MvcConfiguration()
        {
            ConfigurationManager.AppSettings["webpages:Version"] = "1.0.0.0";

            HtmlHelper.ClientValidationEnabled = false;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = false;

            ModelBinders.Binders.DefaultBinder = new SubTypeModelBinder();
            ModelMetadataProviders.Current = new ExtendedModelMetadataProvider();
        }

        public static void AutofacDependencyResolver()
        {
            var builder = new ContainerBuilder();

            foreach (var module in Modules)
            {
                builder.RegisterModule(module);
            }

            builder.RegisterFilterProvider(); // property injection for filter attributes

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }

        public static void IgnoreRoutes()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("favicon.ico");
        }

        public static void Auth()
        {
            Membership.Providers.Clear();
            Membership.Providers.Add(new SimpleMembershipProvider());

            FormsAuthentication.EnableFormsAuthentication(new NameValueCollection() {
                    { "loginUrl", LogOnUrl }
            });

            Roles.Enabled = true;
            Roles.Providers.Clear();
            Roles.Providers.Add(new SimpleRoleProvider());
        }

        public static void DefaultRoute()
        {
            RouteTable.Routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" }
            );
        }

        public static void Bundles()
        {
            BundleTable.Bundles.AddScriptsBundle("bootstrap", "jquery-1.7.2.js", "jquery.validate.js", "bootstrap.js");
            BundleTable.Bundles.AddStylesBundle("bootstrap", "bootstrap.css", "bootstrap-responsive.css"); 
        }

        public static void ErrorHandling()
        {
            //GlobalFilters.Filters.Add(new LogErrorAttribute());
            Startup.Modules.Add(new CommonGlobalFiltersModule()); // autofac approach to global filters            
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }

        // http://blogs.msdn.com/b/marcinon/archive/2011/08/16/optimizing-mvc-view-lookup-performance.aspx
        public static void ViewLocation()
        {
            // limits view location lookups
            ViewEngines.Engines.Clear();

            var razorViewEngine = new RazorViewEngine();
            razorViewEngine.ViewLocationCache = new TwoLevelViewCache(razorViewEngine.ViewLocationCache);

            ViewEngines.Engines.Add(razorViewEngine);
        }

        // http://serverfault.com/questions/24885/how-to-remove-iis-asp-net-response-headers
        public static void RemoveServerFingerprint()
        {
            // limits warnings from automated intrusion detection software
            // [1] add <httpRuntime enableVersionHeader="false"/> to remove X-AspNet-Version header
            // [2] remove X-AspNetMvc-Version header
            MvcHandler.DisableMvcResponseHeader = true;

            // [3] remove all redundant server headers
            DynamicModuleUtility.RegisterModule(typeof(FilterResponseHeadersModule));
        }

        public static void Imaging()
        {
            ImageSizes.Add(ImageSize.Thumb, new Transform(200, 200, false));
        }

        public static void Rewriting()
        {
            DynamicModuleUtility.RegisterModule(typeof(RedirectModule));
        }

        public static void Admin(bool registerNavigationWidget = true)
        {
            Modules.Add(new AuthAreaModule());
            Modules.Add(new EditorAreaModule());
            Modules.Add(new FileSystemModule());

            if (registerNavigationWidget)
            {
                Parts.Add(new NavigationWidget(builder =>
                {
                    builder
                        .Controller<Instatus.Areas.Editor.Controllers.ArticleController>("Pages")
                        .Controller<Instatus.Areas.Editor.Controllers.BrandController>("Brands")
                        .Controller<Instatus.Areas.Editor.Controllers.CatalogController>("Catalogs")
                        .Controller<Instatus.Areas.Editor.Controllers.FileController>("Files")
                        .Controller<Instatus.Areas.Editor.Controllers.NewsController>("News")
                        .Controller<Instatus.Areas.Editor.Controllers.OrganizationController>("Organizations")
                        .Controller<Instatus.Areas.Editor.Controllers.PostController>("Posts")
                        .Controller<Instatus.Areas.Editor.Controllers.ProfileController>("Profiles")
                        .Controller<Instatus.Areas.Editor.Controllers.TagController>("Tags");
                },
                viewName: WebConstant.ViewName.NavBar,
                scope: WebConstant.Scope.Admin));
            }
        }

        public static void Social()
        {
            Modules.Add(new GoogleAreaModule());
            Modules.Add(new FacebookAreaModule());
        }
    }

    // http://code.google.com/p/autofac/wiki/Mvc3Integration
    public class MockServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MockMembershipService>().As<IMembershipService>().InstancePerLifetimeScope();
            builder.RegisterType<InMemoryApplicationModel>().As<IApplicationModel>().InstancePerLifetimeScope();
            builder.RegisterType<InMemoryLoggingService>().As<ILoggingService>().InstancePerLifetimeScope();
        }
    }

    public class DbServicesModule : Module
    {
        public string Alias { get; set; } 

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbMembershipService>().As<IMembershipService>().InstancePerHttpRequest();
            builder.Register(c => new DbApplicationModel(Alias)).As<IApplicationModel>().InstancePerHttpRequest();
            builder.RegisterType<DbLoggingService>().As<ILoggingService>().InstancePerHttpRequest();
            builder.RegisterType<DbPageModel>().As<IPageModel>().InstancePerHttpRequest();
        }

        public DbServicesModule(string alias, IDatabaseInitializer<DbApplicationModel> initializer)
        {
            Alias = alias;

            Database.SetInitializer<DbApplicationModel>(initializer);
        }
    }

    public class FileSystemModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileSystemBlobService>().As<IBlobService>().InstancePerLifetimeScope();
        }
    }

    public class CommonServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RazorTemplateService>().As<ITemplateService>().InstancePerLifetimeScope();
        }
    }

    public class CommonGlobalFiltersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LogErrorAttribute>().As<IMvcFilter>();
        }
    }
}
