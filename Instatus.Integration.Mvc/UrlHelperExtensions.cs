using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Instatus.Core.Extensions;
using Instatus.Core.Utils;
using System.Web;
using System.Net.Http;

namespace Instatus.Integration.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string Page(this UrlHelper urlHelper, string key)
        {
            var locale = DependencyResolver.Current.GetService<IPreferences>().Locale;
            
            return urlHelper.RouteUrl(WellKnown.RouteName.Page, new { locale = locale, key = key });
        }

        public static string CdnContent(this UrlHelper urlHelper, string virtualPath, bool enableCacheBusting = false)
        {
            string uri;

            if (string.IsNullOrEmpty(virtualPath))
            {
                return string.Empty;
            }
            else if (Uri.IsWellFormedUriString(virtualPath, UriKind.Absolute))
            {
                uri = new PathBuilder(virtualPath)
                            .WithCacheBusting(enableCacheBusting)
                            .ToProtocolRelativeUri();
            }
            else
            {
                var hosting = DependencyResolver.Current.GetService<IHosting>();
                var cdnBaseAddress = hosting.GetAppSetting(WellKnown.AppSetting.CdnBaseAddress);
                uri = new PathBuilder(cdnBaseAddress)
                            .Path(virtualPath)
                            .WithCacheBusting(enableCacheBusting)
                            .ToProtocolRelativeUri();
            }
            
            return urlHelper.Content(uri);
        }

        public static string BlobContent(this UrlHelper urlHelper, string virtualPath, bool enableCacheBusting = false)
        {
            string uri;
            
            if (string.IsNullOrEmpty(virtualPath))
            {
                return string.Empty;
            }
            else if (Uri.IsWellFormedUriString(virtualPath, UriKind.Absolute))
            {
                uri = new PathBuilder(virtualPath)
                            .WithCacheBusting(enableCacheBusting)
                            .ToString(); // not protocol relative; for placeholder images
            }
            else
            {
                var blobStorage = DependencyResolver.Current.GetService<IBlobStorage>();
                var blobUri = blobStorage.GenerateUri(virtualPath, HttpMethod.Get);
            
                uri = new PathBuilder(blobUri)
                            .WithCacheBusting(enableCacheBusting)
                            .ToProtocolRelativeUri();
            }

            return urlHelper.Content(uri);
        }
    }
}
