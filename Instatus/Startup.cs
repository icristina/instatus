using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Instatus;
using Instatus.Areas.Auth;
using Instatus.Areas.Developer;
using Instatus.Areas.Editor;
using Instatus.Areas.Facebook;
using Instatus.Areas.Google;
using Instatus.Areas.Moderator;
using Instatus.Data;
using Instatus.Entities;
using Instatus.Models;
using Instatus.Services;
using Instatus.Web;
using Instatus.Widgets;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Instatus.Startup), "PreApplicationStart", Order = 10)]
[assembly: WebActivator.PostApplicationStartMethod(typeof(Instatus.Startup), "PostApplicationStart", Order = 10)]

namespace Instatus
{
    public static class Startup
    {        
        public static List<Module> Modules = new List<Module>();
        public static List<dynamic> Processors = new List<dynamic>();
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
            ViewLocation();
            RemoveServerFingerprint();
            Rewriting();
        }

        private static void PostApplicationStart()
        {
            if (!AutoStartup)
                return;
            
            BuildDependencyContainer();
            ErrorHandling();
            DefaultRoute();
            Bundles();
        }

        public static void RegisterModules(params Module[] modules)
        {
            Modules.AddRange(modules);
        }

        public static void MvcConfiguration()
        {
            ConfigurationManager.AppSettings["webpages:Version"] = "1.0.0.0";

            HtmlHelper.ClientValidationEnabled = false;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = false;

            ModelBinders.Binders.DefaultBinder = new SubTypeModelBinder();
            ModelMetadataProviders.Current = new ExtendedModelMetadataProvider();
        }

        public static void BuildDependencyContainer()
        {
            var builder = new ContainerBuilder();

            foreach (var module in Modules)
            {
                builder.RegisterModule(module);
            }

            builder.RegisterFilterProvider(); // property injection for filter attributes

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));

            GlobalConfiguration.Configuration.DependencyResolver = DependencyResolver.Current.ToWebHttpDependencyResolver(); // web api

            foreach (var processor in Processors) // start after dependency injection container
                processor.Start();
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

        private static string defaultNamespace;

        public static string DefaultNamespace
        {
            get
            {
                if (defaultNamespace.IsEmpty() && Modules.Any())
                    defaultNamespace = Modules.First().GetType().Namespace + ".Controllers";
                
                return defaultNamespace;
            }
            set
            {
                defaultNamespace = value;
            }
        }

        public static void DefaultRoute()
        {
            RouteTable.Routes
                .RemoveRoute("Api")
                .MapHttpRoute( // webapi
                    "Api",
                    "api/{controller}/{id}",
                    new { id = RouteParameter.Optional }
                );            
            
            RouteTable.Routes
                .RemoveRoute("Default")
                .MapRouteLowercase( // mvc
                    "Default",
                    "{controller}/{action}/{id}",
                    new { controller = "Home", action = "Index", id = "" },
                    null,
                    new string[] { 
                        DefaultNamespace
                    }
                );
        }

        public static void Bundles()
        {
            BundleTable.Bundles.AddScriptsBundle("bootstrap", "jquery-1.7.2.js", "jquery.validate.js", "bootstrap.js");
            BundleTable.Bundles.AddStylesBundle("bootstrap", "bootstrap.css", "bootstrap-responsive.css"); 
        }

        public static void ErrorHandling()
        {            
            GlobalFilters.Filters.Add(new LogErrorAttribute());
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

                    if (Modules.OfType<ModeratorAreaModule>().Any())
                        builder
                            .Controller<Instatus.Areas.Moderator.Controllers.ActivityController>("Activities")
                            .Controller<Instatus.Areas.Moderator.Controllers.ExportController>("Export")
                            .Controller<Instatus.Areas.Moderator.Controllers.PostController>("User Generated Content");

                    if (Modules.OfType<DeveloperAreaModule>().Any())
                        builder
                            .Controller<Instatus.Areas.Developer.Controllers.TaxonomyController>("Taxonomies")
                            .Controller<Instatus.Areas.Developer.Controllers.CredentialController>("Credentials")
                            .Controller<Instatus.Areas.Developer.Controllers.DomainController>("Domains")
                            .Controller<Instatus.Areas.Developer.Controllers.LogController>("Logs")
                            .Controller<Instatus.Areas.Developer.Controllers.PhraseController>("Localization")
                            .Controller<Instatus.Areas.Developer.Controllers.RedirectController>("Redirects")
                            .Controller<Instatus.Areas.Developer.Controllers.RegionController>("Regions")
                            .Controller<Instatus.Areas.Developer.Controllers.UserController>("Users");
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

        public static void RegisterMessageProcessor<TMessage>(int retry = 0, int delay = 10000, int parallelism = 1, ILifetimeScope lifetimeScope = null)
        {
            Processors.Add(new ParallelMessageProcessor<TMessage>(retry, delay, parallelism, lifetimeScope));
        }

        public static void RegisterFieldType<T>()
        {
            WebConstant.Serialization.KnownTypes.Add(typeof(T));
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
            builder.RegisterType<DbCredentialService>().As<ICredentialService>().InstancePerHttpRequest();
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
            builder.RegisterType<FileSystemBlobService>()
                .As<IBlobService>()
                .As<ILocalStorageService>()
                .InstancePerLifetimeScope();
        }
    }

    public class CommonServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RazorTemplateService>().As<ITemplateService>().InstancePerLifetimeScope();
        }
    }
}
