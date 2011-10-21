using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Instatus.Data;

namespace Instatus.Web
{
    public static class WebPaths
    {
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