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
    public class AspNetSessionData : ISessionData
    {
        public virtual string SiteName
        {
            get
            {
                return HttpContext.Current.Request.Url.Host;
            }
        }

        private const string cookieKey = "preferences";
        private const string localeKey = "locale";

        private string locale;

        // override, for example if sourced from facebook signed_request, this takes highest priority
        public virtual string GetCustomLocale()
        {
            return null;
        }

        // format en-GB, en-US, de-DE
        public string Locale
        {
            get
            {
                // order of precedence
                // [1] already parsed in memory, requires AspNetSession to be declared as request scope
                // [2] query string or form param
                // [3] cookie
                // [4] accept language from browser
                // [5] thread culture
                if (locale == null)
                {
                    var request = HttpContext.Current.Request;

                    this.Locale = GetCustomLocale() ??
                        request.Params[localeKey] ??
                        (request.Cookies[cookieKey] != null ? request.Cookies[cookieKey][localeKey] : null) ??
                        (request.UserLanguages != null ? request.UserLanguages[0] : null);
                }

                return locale;
            }
            set
            {
                CultureInfo culture;

                try
                {
                    culture = new CultureInfo(value);
                }
                catch
                {
                    culture = Thread.CurrentThread.CurrentCulture;
                }

                HttpContext.Current.Response.Cookies[cookieKey][localeKey] = locale = culture.Name;
            }
        }
    }
}
