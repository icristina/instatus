using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections.Specialized;
using Instatus.Core;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Instatus.Integration.Mvc
{
    public static class BaseMvcConfig
    {
        public static void RegisterIgnoreRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
        }

        public static void RegisterDefaultRoute(RouteCollection routes)
        {
            routes.MapRouteLowercase(
                name: WellKnown.RouteName.Default,
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        public static void RegisterPageRoute(RouteCollection routes, string controllerName = "ContentPage", string defaultLocale = "en-US")
        {
            routes.MapRoute(
                name: WellKnown.RouteName.Page,
                url: "{locale}/{key}",
                defaults: new { controller = controllerName, action = "Details", locale = defaultLocale },
                constraints: new { locale = WellKnown.RegularExpression.Locale }
            );
        }

        public static void RegisterImageHandlerRoute(RouteCollection routes, 
            string url = "cdn/photo/{action}/{width}/{height}/{bucket}/{*pathInfo}",
            IEnumerable<Tuple<int, int>> whiteListDimensions = null)
        {
            routes.Add(new Route(url, new ImageHandler(whiteListDimensions)));
        }

        public static void RegisterMembershipProvider(string loginUrl = "/account/login")
        {
            Membership.Providers.Clear();
            Membership.Providers.Add(new SimpleMembershipProvider());

            FormsAuthentication.EnableFormsAuthentication(new NameValueCollection() 
            {
                { "loginUrl", loginUrl }
            });
        }

        public static void RegisterRoleProvider()
        {
            Roles.Enabled = true;
            Roles.Providers.Clear();
            Roles.Providers.Add(new SimpleRoleProvider());
        }
        
        public static void RegisterExceptionFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogExceptionAttribute());
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterCompactPrivacyPolicyFilters(GlobalFilterCollection filters)
        {
            filters.Add(new IframeCookieSupportAttribute());
        }

        // http://serverfault.com/questions/24885/how-to-remove-iis-asp-net-response-headers
        public static void RegisterRemoveServerFingerprint()
        {
            // limits warnings from automated intrusion detection software
            // [1] add <httpRuntime enableVersionHeader="false"/> to remove X-AspNet-Version header
            // [2] remove X-AspNetMvc-Version header
            MvcHandler.DisableMvcResponseHeader = true;

            // [3] add
            //<system.webServer>
            //  <httpProtocol>
            //    <customHeaders>
            //      <remove name="X-Powered-By" />
            //    </customHeaders>
            //  </httpProtocol>   
            //</system.webServer>            

            // [3] remove Server headers
            DynamicModuleUtility.RegisterModule(typeof(FilterResponseHeadersModule));
        }
    }
}
