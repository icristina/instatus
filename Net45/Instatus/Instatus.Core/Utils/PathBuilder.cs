using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace Instatus.Core.Utils
{
    public class PathBuilder
    {
        private bool forceLowerCasePath = false;
        private bool hasQuery = false;
        private StringBuilder stringBuilder;

        public static readonly char[] RelativeChars = new char[] { '~', '/', '\\' };
        public static readonly char[] DelimiterChars = new char[] { '/', '\\' };

        public PathBuilder Path(string path)
        {
            if (forceLowerCasePath)
                path = path.ToLower();

            path = path.TrimStart('/').TrimEnd('/');
            
            stringBuilder.Append('/');
            stringBuilder.Append(path);
            
            return this;
        }

        public PathBuilder Query(string name, object value) 
        {
            if (!string.IsNullOrWhiteSpace(name) && value != null)
            {
                if (!hasQuery)
                {
                    stringBuilder.Append('?');
                    hasQuery = true;
                }
                else
                {
                    stringBuilder.Append('&');
                }

                if (value.GetType().IsArray)
                    value = string.Join(",", (value as IEnumerable).Cast<object>().ToArray());

                var encodedName = UriQueryUtility.UrlEncode(name);
                var encodedValue = UriQueryUtility.UrlEncode(value.ToString());

                stringBuilder.AppendFormat("{0}={1}", encodedName, encodedValue);
            }

            return this;
        }

        public string ToProtocolRelativeUri()
        {
            var uri = ToString();
            return uri.Substring(uri.IndexOf('/'));
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }

        public PathBuilder(string baseAddress, bool forceLowerCasePath = false)
        {
            this.forceLowerCasePath = forceLowerCasePath;
            
            var queryIndex = baseAddress.LastIndexOf('?');

            hasQuery = (queryIndex >= 0);

            var pathSegment = string.Empty;
            var querySegment = string.Empty;

            if (hasQuery)
            {
                pathSegment = baseAddress.Substring(0, queryIndex).TrimEnd('/');
                querySegment = baseAddress.Substring(queryIndex);
            }
            else
            {
                pathSegment = baseAddress.TrimEnd('/');
            }

            stringBuilder = new StringBuilder(forceLowerCasePath ? pathSegment.ToLower() : pathSegment)
                .Append(querySegment);
        }
    }
}
