using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus
{
    // http://stackoverflow.com/questions/1188096/truncating-query-string-returning-clean-url-c-sharp-asp-net/1188180#1188180
    public static class UriExtensions
    {
        public static string RemoveQueryString(this Uri uri)
        {
            return uri.GetLeftPart(UriPartial.Path);
        }
    }
}