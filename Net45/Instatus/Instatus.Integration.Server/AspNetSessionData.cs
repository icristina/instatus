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

namespace Instatus.Integration.Server
{
    public class AspNetSessionData : ISessionData
    {
        private ILocalization localization;
        
        public virtual string SiteName
        {
            get
            {
                return HttpContext.Current.Request.Url.Host;
            }
        }

        private string locale;

        // override, for example if sourced from facebook signed_request, this takes highest priority
        public virtual string GetCustomLocale(HttpRequest request)
        {
            return null;
        }

        public string GetRouteLocale(RouteData routeData)
        {
            return routeData.Values.GetValue<string>(WellKnown.RouteValue.Locale);
        }

        public string GetParamsLocale(HttpRequest request)
        {
            return request.Params[WellKnown.Preference.Locale];
        }

        public string GetCookieLocale(HttpRequest request)
        {
            return request.Cookies.GetValue<string>(WellKnown.Cookie.Preferences, WellKnown.Preference.Locale);
        }

        public string GetAcceptLanguage(HttpRequest request)
        {
            return request.UserLanguages == null ? null 
                : request.UserLanguages[0];
        }

        public virtual string GetDefaultLocale() 
        {
            return Thread.CurrentThread.CurrentUICulture.Name;
        }

        public string Locale
        {
            get
            {
                // order of precedence
                // [1] already parsed in memory, requires AspNetSession to be declared as request scope
                // [2] query string or form param
                // [3] route data
                // [4] cookie
                // [5] accept language from browser
                // [6] default or thread culture
                if (locale == null)
                {
                    var request = HttpContext.Current.Request;
                    var routeData = request.RequestContext.RouteData;

                    this.Locale = GetCustomLocale(request) ??
                        GetParamsLocale(request) ??
                        GetRouteLocale(routeData) ??                        
                        GetCookieLocale(request) ??
                        GetAcceptLanguage(request) ??
                        GetDefaultLocale();
                }

                return locale;
            }
            set
            {
                try
                {
                    var culture = CultureInfo.GetCultureInfo(value);

                    if (!localization.SupportedCultures.Contains(culture))
                    {
                        culture = localization.SupportedCultures.First();
                    }

                    var request = HttpContext.Current.Request;
                    var response = HttpContext.Current.Response;

                    Thread.CurrentThread.CurrentUICulture = culture;
                    locale = culture.Name;
                    response.Cookies.SetValue(WellKnown.Cookie.Preferences, WellKnown.Preference.Locale, locale);

                    var routeLocale = GetRouteLocale(request.RequestContext.RouteData);

                    if (routeLocale != null && locale != routeLocale)
                    {
                        var redirectRouteData = new RouteValueDictionary(request.RequestContext.RouteData.Values);

                        redirectRouteData[WellKnown.RouteValue.Locale] = locale;

                        response.RedirectToRoute(redirectRouteData);
                    }
                }
                catch
                {

                }
            }
        }

        public AspNetSessionData(ILocalization localization)
        {
            this.localization = localization;
        }
    }
}
