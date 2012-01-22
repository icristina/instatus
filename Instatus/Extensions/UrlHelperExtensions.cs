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
        public static string Relative(this UrlHelper urlHelper, string virtualPath)
        {
            return WebPath.Relative(virtualPath);
        }

        public static string Absolute(this UrlHelper urlHelper, string virtualPath)
        {
            return WebPath.Absolute(virtualPath);
        }

        public static string Absolute(this UrlHelper urlHelper, string actionName, string controllerName, object routeValues)
        {
            return WebPath.Absolute(urlHelper.Action(actionName, controllerName, routeValues));
        }

        public static string Absolute(this UrlHelper urlHelper, string routeName, object routeValues)
        {
            return WebPath.Absolute(urlHelper.RouteUrl(routeName, routeValues));
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
            if (slug.Match(RouteCollectionExtensions.HomeSlug))
                return urlHelper.RouteUrl(RouteCollectionExtensions.HomeRouteName);
            
            return urlHelper.RouteUrl(RouteCollectionExtensions.NavigableRouteName, new { slug = slug });
        }

        public static string Self(this UrlHelper urlHelper, IWebView webView)
        {
            var routeData = urlHelper.RequestContext.RouteData;
            return urlHelper.Action(routeData.ActionName(), routeData.ControllerName(), webView.Query.ToRouteValueDictionary());
        }

        public static SiteMapNodeCollectionBuilder Nav(this UrlHelper urlHelper, string title, string slug)
        {
            return new SiteMapNodeCollectionBuilder(urlHelper)
                        .Nav(title, slug);
        }

        public static SiteMapNodeCollectionBuilder Nav(this UrlHelper urlHelper, string title, string actionName, string controllerName)
        {
            return new SiteMapNodeCollectionBuilder(urlHelper)
                        .Nav(title, actionName, controllerName);
        }
    }
}

namespace Instatus.Web
{
    public class SiteMapNodeCollectionBuilder {
        private UrlHelper urlHelper;
        private List<SiteMapNode> siteMapNodes;
        private SiteMapProvider siteMapProvider;

        public SiteMapNodeCollectionBuilder Nav(string title, string slug)
        {
            siteMapNodes.Add(new SiteMapNode(siteMapProvider, slug, urlHelper.Page(slug), title));
            return this;
        }

        public SiteMapNodeCollectionBuilder Nav(string title, string actionName, string controllerName)
        {
            siteMapNodes.Add(new SiteMapNode(siteMapProvider, controllerName, urlHelper.Action(actionName, controllerName), title));
            return this;
        }

        public SiteMapNodeCollection ToSiteMapNodeCollection()
        {           
            return new SiteMapNodeCollection(siteMapNodes.ToArray());
        }

        public SiteMapNodeCollectionBuilder(UrlHelper urlHelper, SiteMapProvider siteMapProvider = null, List<SiteMapNode> siteMapNodes = null)
        {
            this.urlHelper = urlHelper;
            this.siteMapProvider = siteMapProvider ?? new SimpleSiteMapProvider();
            this.siteMapNodes = siteMapNodes ?? new List<SiteMapNode>();
        }
    }
}