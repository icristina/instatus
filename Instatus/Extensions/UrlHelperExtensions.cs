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
            return WebPath.Relative(WebPath.Resize(size, virtualPath));
        }

        public static SiteMapNodeCollection Controllers(this UrlHelper urlHelper, Func<ControllerDescriptor, bool> isNavigable = null, string actionName = "Index")
        {
            var siteMapProvider = new SimpleSiteMapProvider();
            var siteMapNodes = MefDependencyResolver
                    .GetTypes<IController>()
                    .Select(controllerType =>
                    {
                        var descriptor = new ReflectedControllerDescriptor(controllerType);
                        var description = descriptor.Description();

                        if ((isNavigable != null && !isNavigable(descriptor)) || !descriptor.HasAction(actionName) || description.IsEmpty())
                            return null;

                        var url = urlHelper.Action(actionName, descriptor.ControllerName);

                        return new SiteMapNode(siteMapProvider, controllerType.Name, url, description);
                    })
                    .RemoveNulls()
                    .ToArray();

            return new SiteMapNodeCollection(siteMapNodes);
        }

        public static string Page(this UrlHelper urlHelper, string slug)
        {
            if (slug.Match(WebRoute.HomeSlug))
                return urlHelper.RouteUrl(WebRoute.Home);
            
            return urlHelper.RouteUrl(WebRoute.Page, new { slug = slug });
        }

        public static string Self(this UrlHelper urlHelper)
        {
            return urlHelper.RequestContext.HttpContext.Request.Url.AbsoluteUri;
        }

        public static string Self(this UrlHelper urlHelper, IWebView webView)
        {
            var routeData = urlHelper.RequestContext.RouteData;
            return urlHelper.Action(routeData.ActionName(), routeData.ControllerName(), webView.Query.ToRouteValueDictionary());
        }

        public static SiteMapNodeCollectionBuilder Navigation(this UrlHelper urlHelper)
        {
            return new SiteMapNodeCollectionBuilder(urlHelper);
        }
    }
}

namespace Instatus.Web
{
    public class SiteMapNodeCollectionBuilder {
        private UrlHelper urlHelper;
        private List<SiteMapNode> siteMapNodes;
        private SiteMapProvider siteMapProvider;

        public SiteMapNodeCollectionBuilder Page(string title, string slug)
        {
            siteMapNodes.Add(new SiteMapNode(siteMapProvider, slug, urlHelper.Page(slug), title));
            return this;
        }

        public SiteMapNodeCollectionBuilder Action(string title, string actionName, string controllerName)
        {
            siteMapNodes.Add(new SiteMapNode(siteMapProvider, controllerName, urlHelper.Action(actionName, controllerName), title));
            return this;
        }

        public SiteMapNodeCollectionBuilder Route(string title, string routeName)
        {
            siteMapNodes.Add(new SiteMapNode(siteMapProvider, routeName, urlHelper.RouteUrl(routeName), title));
            return this;
        }

        public SiteMapNodeCollectionBuilder External(string title, string uri)
        {
            siteMapNodes.Add(new SiteMapNode(siteMapProvider, uri, uri, title));
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