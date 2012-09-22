using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Instatus.Core.Extensions;

namespace Instatus.Integration.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string ContentPage(this UrlHelper urlHelper, string key)
        {
            var locale = CultureInfo.CreateSpecificCulture(DependencyResolver.Current.GetService<ISessionData>().Locale);
            
            return urlHelper.RouteUrl(WellKnown.RouteName.ContentPage, new { language = locale.TwoLetterISOLanguageName, country = locale.TwoLetterCountryCode(), key = key });
        }
    }
}
