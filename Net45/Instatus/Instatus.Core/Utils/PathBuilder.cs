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
        private bool hasQuery = false;
        private StringBuilder stringBuilder;

        public PathBuilder Path(string path)
        {
            stringBuilder.Append('/');
            stringBuilder.Append(path.TrimStart('/').TrimEnd('/'));
            
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

        public PathBuilder(string baseAddress)
        {
            hasQuery = baseAddress.Contains('?');
            stringBuilder = new StringBuilder(baseAddress.TrimEnd('/'));
        }
    }
}
