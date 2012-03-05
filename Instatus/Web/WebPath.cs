using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Instatus.Data;
using Instatus.Services;
using System.Web.Mvc;
using System.Web.Routing;
using Instatus.Models;

namespace Instatus.Web
{
    public static class WebPath
    {
        static WebPath()
        {
            WebApp.OnReset(() => baseUri = null);
        }                
        
        private static Uri baseUri;

        public static Uri BaseUri
        {
            get
            {
                if (baseUri.IsEmpty())
                {
                    using (var db = WebApp.GetService<IApplicationContext>())
                    {
                        if (db == null && HttpContext.Current.Request != null)
                        {
                            baseUri = new Uri(HttpContext.Current.Request.BaseUri());
                        }
                        else
                        {
                            var domain = db.GetApplicationDomain();

                            if (!domain.IsEmpty())
                            {
                                baseUri = domain.Uri.StartsWith("http://") ? new Uri(domain.Uri) : new Uri("http://" + domain.Uri);
                            }
                            else if (HttpContext.Current.Request != null)
                            {
                                baseUri = new Uri(HttpContext.Current.Request.BaseUri());
                            }
                        }
                    }
                }

                return baseUri;
            }
        }

        public static string Server(string virtualPath)
        {
            return HttpContext.Current.Server.MapPath(virtualPath);
        }

        public static string Relative(string virtualPath)
        {
            foreach(var virtualPathRewriter in WebApp.GetServices<IVirtualPathRewriter>()) {
                virtualPath = virtualPathRewriter.RewriteVirtualPath(virtualPath);
            }

            if(virtualPath.IsAbsoluteUri())
                return virtualPath;

            if (!virtualPath.StartsWith("~/"))
                virtualPath = string.Format("~/{0}", virtualPath);

            return VirtualPathUtility.ToAbsolute(virtualPath).ToLower();
        }

        public static string Absolute(Uri baseUri, string virtualPath)
        {
            return new Uri(baseUri, Relative(virtualPath)).ToString();
        }

        public static string Absolute(string baseUri, string virtualPath)
        {
            return Absolute(new Uri(baseUri), virtualPath);
        }

        public static string Absolute(string virtualPath)
        {
            return virtualPath.IsAbsoluteUri() ? virtualPath : Absolute(BaseUri, virtualPath);
        }

        public static string ProtocolRelative(string absoluteUrl)
        {
            return HttpContext.Current.Request != null ? "//" + absoluteUrl.SubstringAfter("//") : absoluteUrl; // if web request, allow protocol relative syntax
        }

        public static string MatchProtocol(string absoluteUrl)
        {
            if (HttpContext.Current.Request == null)
                return absoluteUrl;            
            
            var uri = new Uri(absoluteUrl);
            var uriBuilder = new UriBuilder(uri);
            var port = HttpContext.Current.Request.Url.Port;

            if (HttpContext.Current.Request.IsSecureConnection)
            {
                uriBuilder.Scheme = "https";

                if(port == 443)
                    uriBuilder.Port = 443;
            }
            else
            {
                uriBuilder.Scheme = "http";

                if (port == 80)
                    uriBuilder.Port = 80;
            }

            return uriBuilder.Uri.ToString(); // user uri property to ensure :80 or :443 default ports not returned
        }

        public static string Resize(WebSize size, string virtualPath)
        {
            if (size == WebSize.Original)
            {
                return virtualPath;
            }
            else
            {
                var extensionStartIndex = virtualPath.LastIndexOf('.');
                var suffix = string.Format("-{0}", size.ToString().ToLower());
                return virtualPath.Insert(extensionStartIndex, suffix);
            }
        }

        public static string ResizeAbsolute(WebSize size, string virtualPath)
        {
            return Absolute(Resize(size, virtualPath));
        }

        public static string Home {
            get 
            {
                //var route = RouteTable.Routes[WebRoute.Home];
                //return route != null ? route.GetVirtualPath(null, null).VirtualPath : "/";
                return "/";
            }
        }
    }
}