using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;
using System.Web.Routing;

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
            var lang = routeData.Values[WellKnown.RouteValue.Language];
            var country = routeData.Values[WellKnown.RouteValue.Country];
            
            return lang == null || country == null ? null 
                : string.Format("{0}-{1}", lang, country);
        }

        public string GetParamsLocale(HttpRequest request)
        {
            return request.Params[WellKnown.Preference.Locale];
        }

        public string GetCookieLocale(HttpRequest request)
        {
            var cookie = request.Cookies[WellKnown.Cookie.Preferences];
            
            return cookie == null ? null 
                : cookie[WellKnown.Preference.Locale];
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
                // [2] route data
                // [3] query string or form param
                // [4] cookie
                // [5] accept language from browser
                // [6] default or thread culture
                if (locale == null)
                {
                    var request = HttpContext.Current.Request;
                    var routeData = request.RequestContext.RouteData;

                    this.Locale = GetCustomLocale(request) ??
                        GetRouteLocale(routeData) ??
                        GetParamsLocale(request) ??
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
                    var culture = CultureInfo.CreateSpecificCulture(value); // specific, in format en-GB, en-US, de-DE

                    if (!localization.SupportedCultures.Contains(culture))
                    {
                        culture = localization.SupportedCultures.First();
                    }

                    var response = HttpContext.Current.Response;

                    Thread.CurrentThread.CurrentUICulture = culture;
                    response.Cookies[WellKnown.Cookie.Preferences][WellKnown.Preference.Locale] = locale = culture.Name;
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
