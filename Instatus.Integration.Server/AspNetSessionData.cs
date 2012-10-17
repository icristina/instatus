using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;
using System.Web.Routing;
using Instatus.Core.Extensions;
using System.Web.Helpers;

namespace Instatus.Integration.Server
{
    public class AspNetpreferences : IPreferences
    {
        public IHosting hosting;
        
        public virtual string GetCustomLocale(HttpRequest request)
        {
            return null;
        }

        private string locale;

        public string Locale
        {
            get
            {
                // order of precedence
                // [1] already parsed in memory, requires AspNetSession to be declared as request scope
                // [2] query string or form param
                // [3] route data
                // [4] cookie
                // [5] custom input
                // [6] accept language from browser
                // [7] default or thread culture
                if (locale == null)
                {
                    var request = HttpContext.Current.Request;
                    var routeData = request.RequestContext.RouteData;

                    this.Locale =
                        request.Unvalidated(WellKnown.RouteValue.Locale) ??
                        routeData.Values.GetValue<string>(WellKnown.RouteValue.Locale) ??
                        request.Cookies.GetValue<string>(WellKnown.Cookie.Preferences, WellKnown.Preference.Locale) ??
                        GetCustomLocale(request) ??
                        (request.UserLanguages == null ? null : request.UserLanguages[0]) ??
                        hosting.DefaultCulture.Name;
                }

                return locale;
            }
            set
            {
                CultureInfo culture;

                try
                {
                    culture = CultureInfo.GetCultureInfo(value);

                    if (!hosting.SupportedCultures.Contains(culture))
                    {
                        culture = hosting.DefaultCulture;
                    }
                }
                catch
                {
                    culture = hosting.DefaultCulture;
                }

                var request = HttpContext.Current.Request;
                var response = HttpContext.Current.Response;
                var routeData = request.RequestContext.RouteData;

                Thread.CurrentThread.CurrentUICulture = culture;

                locale = culture.Name;
                response.Cookies.SetValue(WellKnown.Cookie.Preferences, WellKnown.Preference.Locale, locale);

                var routeLocale = routeData.Values.GetValue<string>(WellKnown.RouteValue.Locale);

                if (routeLocale != null && locale != routeLocale)
                {
                    var redirectRouteData = new RouteValueDictionary(request.RequestContext.RouteData.Values);

                    redirectRouteData[WellKnown.RouteValue.Locale] = locale;

                    response.RedirectToRoute(redirectRouteData);
                }
            }
        }

        public AspNetpreferences(IHosting hosting)
        {
            this.hosting = hosting;
        }
    }
}
