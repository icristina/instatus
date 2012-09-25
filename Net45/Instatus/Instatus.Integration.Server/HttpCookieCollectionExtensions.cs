using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Instatus.Integration.Server
{
    public static class HttpCookieCollectionExtensions
    {
        public static T GetValue<T>(this HttpCookieCollection cookies, string cookieName, string key)
        {
            var cookie = cookies[cookieName];

            if (cookie != null)
            {
                try
                {
                    return (T)Convert.ChangeType(cookie[key], typeof(T));
                }
                catch
                {
                    return default(T);
                }
            }

            return default(T);
        }

        public static void SetValue(this HttpCookieCollection cookies, string cookieName, string key, object value)
        {
            var cookie = cookies[cookieName];
            
            cookie[key] = value.ToString();
            cookie.Expires = DateTime.Now.AddYears(1);
        }
    }
}
