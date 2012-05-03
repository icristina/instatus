using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Web;
using Instatus.Data;
using System.Configuration;
using System.ComponentModel;
using Instatus.Models;

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

        public static string Resize(this UrlHelper urlHelper, ImageSize size, string virtualPath)
        {
            return WebPath.Relative(WebPath.Resize(size, virtualPath));
        }

        public static string Details(this UrlHelper urlHelper, int id)
        {
            return urlHelper.Action("Details", new { id = id });
        }

        public static string Page(this UrlHelper urlHelper, string slug)
        {
            if (slug.Match(WebConstant.Alias.Home))
                return urlHelper.RouteUrl(WebConstant.Route.Home);
            
            return urlHelper.RouteUrl(WebConstant.Route.Page, new { slug = slug });
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

        public static SiteMapNodeCollectionBuilder SiteMapBuilder(this UrlHelper urlHelper)
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

        public SiteMapNodeCollectionBuilder Action(string title, string actionName, string controllerName, string area = null)
        {
            siteMapNodes.Add(new SiteMapNode(siteMapProvider, controllerName, urlHelper.Action(actionName, controllerName, new { area = area }), title));
            return this;
        }

        public SiteMapNodeCollectionBuilder Controller<T>(string title = null, string actionName = "Index") where T : Controller
        {
            var controllerType = typeof(T);
            var descriptor = new ReflectedControllerDescriptor(controllerType);
            var description = title ?? controllerType.GetCustomAttributeValue<DescriptionAttribute, string>(d => d.Description);
            var roles = controllerType.GetCustomAttributeValue<AuthorizeAttribute, string>(a => a.Roles).ToList();
            var user = urlHelper.RequestContext.HttpContext.User;

            if (!descriptor.GetCanonicalActions().Any(a => a.ActionName == actionName)
                  || (title.IsEmpty() && description.IsEmpty())
                  || (!roles.IsEmpty() && !roles.Any(r => user.IsInRole(r)))) // required role
                return this;

            var url = urlHelper.Action(actionName, descriptor.ControllerName, new { area = controllerType.GetNamespaceByConvention() });

            siteMapNodes.Add(new SiteMapNode(siteMapProvider, descriptor.ControllerName, url, description));

            return this;
        }

        public SiteMapNodeCollectionBuilder Route(string title, string routeName)
        {
            siteMapNodes.Add(new SiteMapNode(siteMapProvider, routeName, urlHelper.RouteUrl(routeName), title));
            return this;
        }

        public SiteMapNodeCollectionBuilder Home(string title)
        {
            siteMapNodes.Add(new SiteMapNode(siteMapProvider, WebConstant.Route.Home, WebPath.Home, title));
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