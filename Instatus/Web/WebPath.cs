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
using System.IO;
using Instatus.Entities;

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
                    var applicationModel = DependencyResolver.Current.GetService<IApplicationModel>();

                    if (applicationModel == null && HttpContext.Current.Request != null)
                    {
                        baseUri = new Uri(HttpContext.Current.Request.BaseUri());
                    }
                    else
                    {
                        var domain = applicationModel.Domains.Where(d => d.Canonical).FirstOrDefault();

                        if (!domain.IsEmpty())
                        {
                            baseUri = domain.Hostname.StartsWith("http://") ? new Uri(domain.Hostname) : new Uri("http://" + domain.Hostname);
                        }
                        else if (HttpContext.Current.Request != null)
                        {
                            baseUri = new Uri(HttpContext.Current.Request.BaseUri());
                        }
                    }
                }

                return baseUri;
            }
        }

        public static string Server(string virtualPath)
        {
            if (!VirtualPathUtility.IsAppRelative(virtualPath))
                return virtualPath;

            if (HttpContext.Current == null)
                return AppDomain.CurrentDomain.BaseDirectory + virtualPath.Replace("~", "");

            return HttpContext.Current.Server.MapPath(virtualPath);
        }

        public static string LowerCasePath(string virtualPath) // do not lowercase query parameters
        {
            if (virtualPath.Contains('?'))
            {
                var path = virtualPath.SubstringBefore("?");
                var query = virtualPath.SubstringAfter("?");

                return string.Format("{0}?{1}", path.ToLowerInvariant(), query);
            }
            else
            {
                return virtualPath.ToLowerInvariant();
            }
        }

        public static string Relative(string virtualPath)
        {
            foreach(var virtualPathRewriter in WebApp.GetServices<IVirtualPathRewriter>()) {
                virtualPath = virtualPathRewriter.RewriteVirtualPath(virtualPath);
            }

            if(virtualPath.IsAbsoluteUri())
                return virtualPath;

            if (!VirtualPathUtility.IsAppRelative(virtualPath))
                virtualPath = string.Format("~/{0}", virtualPath);

            virtualPath = VirtualPathUtility.ToAbsolute(virtualPath);

            return LowerCasePath(virtualPath);
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

        public static string Variant(string virtualPath, string suffix, char seperator = '.', string extension = null)
        {
            var extensionStartIndex = virtualPath.LastIndexOf('.');
            var suffixAndSeperator = string.Format("-{0}", suffix.ToLower());
            var ammendedVirtualPath = virtualPath.Insert(extensionStartIndex, suffixAndSeperator);

            if (extension != null)
                return Path.ChangeExtension(ammendedVirtualPath, extension);

            return ammendedVirtualPath;
        }

        public static string Resize(ImageSize size, string virtualPath, bool normalizeExtension = true)
        {
            if (size == ImageSize.Original)
            {
                return virtualPath;
            }
            else
            {
                return Variant(virtualPath, size.ToString(), extension: normalizeExtension ? "jpg" : null);
            }
        }

        public static string ResizeAbsolute(ImageSize size, string virtualPath)
        {
            return Absolute(Resize(size, virtualPath));
        }

        public static string Home {
            get 
            {
                return "/";
            }
        }
    }
}