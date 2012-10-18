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

namespace Instatus.Integration.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string ContentPage(this UrlHelper urlHelper, string key)
        {
            var locale = DependencyResolver.Current.GetService<IPreferences>().Locale;
            
            return urlHelper.RouteUrl(WellKnown.RouteName.ContentPage, new { locale = locale, key = key });
        }

        public static string CdnContent(this UrlHelper urlHelper, string virtualPath, bool enableCacheBusting = false)
        {
            var hosting = DependencyResolver.Current.GetService<IHosting>();
            var cdnBaseAddress = hosting.GetAppSetting(WellKnown.AppSetting.CdnBaseAddress);
            var applicationAssembly = HttpContext.Current.ApplicationInstance.GetType().Assembly;
            var uri = new PathBuilder(cdnBaseAddress)
                                .Path(virtualPath)
                                .WithCacheBusting(enableCacheBusting)
                                .ToProtocolRelativeUri();

            return urlHelper.Content(uri);
        }
    }
}
