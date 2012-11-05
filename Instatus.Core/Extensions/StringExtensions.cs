using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Extensions
{
    public static class StringExtensions
    {
        public static IDictionary<string, object> AsDictionary(this string input)
        {
            var values = new Dictionary<string, object>();
            var segments = input.Split(';');

            foreach (var setting in segments.Where(s => s.Length >= 3))
            {
                var startIndex = setting.IndexOf('=');
                var key = setting.Substring(0, startIndex);
                var value = setting.Substring(startIndex + 1);

                values.Add(key.Trim(), value.Trim());
            }

            return values;
        }

        public static string WithNamespace(this string key, string ns)
        {
            return ns.ToLower().Trim() + ":"  + key.Trim();
        }

        public static string WithLocale(this string key, string locale)
        {
            return key + "." + locale;
        }
    }
}
