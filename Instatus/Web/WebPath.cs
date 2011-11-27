using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Instatus.Data;
using Instatus.Services;

namespace Instatus.Web
{
    public static class WebPath
    {
        static WebPath()
        {
            PubSub.Provider.Subscribe<ApplicationReset>(a =>
            {
                baseUri = null;
            });
        }                
        
        private static Uri baseUri;

        public static Uri BaseUri
        {
            get
            {
                if (baseUri.IsEmpty())
                {
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
            if (Uri.IsWellFormedUriString(virtualPath, UriKind.Absolute))
                return virtualPath;

            return Absolute(BaseUri, virtualPath);
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