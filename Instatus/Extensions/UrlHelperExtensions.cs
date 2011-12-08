using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Web;
using Instatus.Data;
using System.Configuration;
using System.ComponentModel;
using Instatus.Areas.Microsite;
using Instatus.Areas.Microsite.Controllers;

namespace Instatus
{
    public static class UrlHelperExtensions
    {
        public static string Absolute(string virtualPath)
        {
            return WebPath.Absolute(virtualPath);
        }
        
        public static string Resize(this UrlHelper urlHelper, WebSize size, string virtualPath)
        {
            return urlHelper.Content(WebPath.Resize(size, virtualPath));
        }

        public static SiteMapNodeCollection SitemapNodes(this UrlHelper urlHelper, Func<ControllerDescriptor, bool> isNavigable = null, string actionName = "Index")
        {
            if (isNavigable == null)
            {
                isNavigable = new Func<ControllerDescriptor, bool>((d) =>
                {
                    return true;
                });
            }

            var siteMapProvider = new SimpleSiteMapProvider();
            var siteMapNodes = MefDependencyResolver
                    .GetTypes<IController>()
                    .Select(controllerType =>
                    {
                        var descriptor = new ReflectedControllerDescriptor(controllerType);
                        var descriptionAttribute = descriptor.GetAttribute<DescriptionAttribute>();

                        if (!isNavigable(descriptor) || !descriptor.HasAction(actionName) || descriptionAttribute == null)
                            return null;

                        var url = urlHelper.Action(actionName, descriptor.ControllerName);

                        return new SiteMapNode(siteMapProvider, controllerType.Name, url, descriptionAttribute.Description);
                    })
                    .Where(s => !s.IsEmpty())
                    .ToArray();

            return new SiteMapNodeCollection(siteMapNodes);
        }

        public static string Page(this UrlHelper urlHelper, string slug)
        {
            if (slug == PageController.HomeSlug)
                return urlHelper.RouteUrl(MicrositeAreaRegistration.RootRouteName);
            
            return urlHelper.RouteUrl(MicrositeAreaRegistration.PageRouteName, new { slug = slug });
        }

        public static string Self(this UrlHelper urlHelper, IWebView webView)
        {
            var routeData = urlHelper.RequestContext.RouteData;
            return urlHelper.Action(routeData.ActionName(), routeData.ControllerName(), webView.Query.ToRouteValueDictionary());
        }
    }
}