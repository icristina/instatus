using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Collections.Specialized;

namespace Instatus
{
    public static class UriBuilderExtensions
    {
        public static string ToQueryString(this IDictionary<string, object> values)
        {
            var queryString = HttpUtility.ParseQueryString("");

            foreach (var key in values.Keys)
            {
                queryString[key] = values[key].ToString();
            }

            return queryString.ToString();
        }
        
        public static UriBuilder Query(this UriBuilder uriBuilder, IDictionary<string, object> values)
        {
            uriBuilder.Query = values.ToQueryString();
            return uriBuilder;
        }        
        
        public static UriBuilder Query(this UriBuilder uriBuilder, NameValueCollection values)
        {
            var queryString = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var key in values.AllKeys)
            {
                queryString[key] = values[key];
            }

            uriBuilder.Query = queryString.ToString();

            return uriBuilder;
        }

        public static UriBuilder Query(this UriBuilder uriBuilder, object values)
        {
            return Query(uriBuilder, new RouteValueDictionary(values).ToNameValueCollection());
        }
        
        public static UriBuilder Query(this UriBuilder uriBuilder, string key, object value)
        {
            return Query(uriBuilder, new NameValueCollection() { { key, value == null ? string.Empty : value.ToString()  } });
        }

        public static UriBuilder QueryFormat(this UriBuilder uriBuilder, string key, string format, params object[] values)
        {
            return uriBuilder.Query(key, string.Format(format, values));
        }

        public static Uri Query(this Uri uri, object values)
        {
            return new UriBuilder(uri).Query(values).Uri;
        }

        public static Uri Query(this Uri uri, string key, object value)
        {
            return new UriBuilder(uri).Query(key, value).Uri;
        }

        public static Uri QueryFormat(this Uri uri, string key, string format, params object[] values)
        {
            return new UriBuilder(uri).QueryFormat(key, format, values).Uri;
        }
    }
}