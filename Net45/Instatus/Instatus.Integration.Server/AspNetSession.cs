using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;

namespace Instatus.Integration.Server
{
    public class AspNetSession : ISession
    {
        public string SiteName
        {
            get 
            {
                return HttpContext.Current.Request.Url.Host;
            }
        }

        private const string cookieKey = "userPreferences";
        private const string localeKey = "locale";

        private string locale;

        // override, for example if sourced from facebook signed_request, this takes highest priority
        public virtual string GetCustomLocale() 
        {
            return null;
        }

        // format en_GB, en_US
        public string Locale
        {
            get
            {
                // order of precedence
                // [1] already parsed in memory, requires AspNetSession to be declared as request scope
                // [2] query string
                // [3] cookie
                // [4] thread culture
                if (locale == null)
                {
                    var request = HttpContext.Current.Request;
                    var localeCustomValue = GetCustomLocale();
                    var localeRequestValue = request.Params[localeKey];
                    var localeCookieValue = request.Cookies[cookieKey][localeKey];

                    CultureInfo culture = Thread.CurrentThread.CurrentCulture;

                    try
                    {
                        culture = new CultureInfo(GetCustomLocale() ?? localeRequestValue ?? localeCookieValue);
                        Thread.CurrentThread.CurrentCulture = culture;
                    }
                    catch
                    {
                        // use default culture
                    }

                    locale = culture.Name;

                    PersistLocale();
                }

                return locale;
            }
            set
            {
                if (!locale.Equals(value))
                {
                    locale = value;
                    PersistLocale();
                }
            }
        }

        private void PersistLocale()
        {
            var response = HttpContext.Current.Response;
            response.Cookies[cookieKey][localeKey] = locale;
        }
    }
}
