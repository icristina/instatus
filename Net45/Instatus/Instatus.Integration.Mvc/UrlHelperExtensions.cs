using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string ContentPage(this UrlHelper urlHelper, string key)
        {
            return urlHelper.RouteUrl(WellKnown.RouteName.ContentPage, new { key = key });
        }
    }
}
