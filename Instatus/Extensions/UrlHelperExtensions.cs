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

namespace Instatus
{
    public static class UrlHelperExtensions
    {
        public static string Absolute(string virtualPath)
        {
            return Paths.Absolute(virtualPath);
        }
        
        public static string Resize(this UrlHelper urlHelper, WebSize size, string virtualPath)
        {
            return urlHelper.Content(Paths.Resize(size, virtualPath));
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

                        if (!isNavigable(descriptor) || !descriptor.HasAction(actionName))
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
            return urlHelper.RouteUrl(MicrositeAreaRegistration.PageRouteName, new { slug = slug });
        }
    }

    public static class Paths
    {
        private static Uri baseUri;
        
        public static Uri BaseUri {
            get {
                if(baseUri.IsEmpty()) {
                    var environment = ConfigurationManager.AppSettings.Value<string>("Environment");
                    using (var db = BaseDataContext.Instance())
                    {
                        var domain = db.Domains.FirstOrDefault(d => d.Environment == environment);

                        if (!domain.IsEmpty())
                            baseUri = new Uri(domain.Uri);
                        else if (HttpContext.Current.Request != null)
                            baseUri = new Uri(HttpContext.Current.Request.BaseUri());
                    }
                }

                return baseUri;
            }
        }

        public static string Absolute(Uri baseUri, string virtualPath)
        {
            return new Uri(baseUri, VirtualPathUtility.ToAbsolute(virtualPath)).ToString();
        }

        public static string Absolute(string baseUri, string virtualPath)
        {
            return Absolute(new Uri(baseUri), virtualPath);
        }

        public static string Absolute(string virtualPath)
        {
            return Absolute(BaseUri, virtualPath);
        }

        public static string Resize(WebSize size, string virtualPath)
        {
            var extensionStartIndex = virtualPath.LastIndexOf('.');
            var suffix = string.Format("-{0}", size.ToString().ToLower());
            return virtualPath.Insert(extensionStartIndex, suffix);
        }

        public static string ResizeAbsolute(WebSize size, string virtualPath)
        {
            return Absolute(Resize(size, virtualPath));
        }
    }
}