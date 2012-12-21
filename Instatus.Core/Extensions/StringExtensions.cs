using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        public static string[] AsDistinctArray(this string input, char[] seperators = null)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new string[] {};

            return input
                .Split(seperators ?? new char[] { ',', ';' })
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .ToArray();
        }

        // https://github.com/erichexter/twitter.bootstrap.mvc/blob/master/src/BootstrapSupport/ViewHelperExtensions.cs
        public static string ToSeparatedWords(this string value)
        {
            return Regex.Replace(value, "([A-Z][a-z])", " $1").Trim();
        }

        private static CultureInfo enUsCulture = new CultureInfo("en-US");

        public static string ToTitleCase(this string value)
        {
            return enUsCulture.TextInfo.ToTitleCase(value);
        }

        public static string WithNamespace(this string key, string ns)
        {
            return ns.ToLower().Trim() + ":"  + key.Trim();
        }

        public static string WithLocale(this string key, string locale)
        {
            return key + "." + locale;
        }

        public static bool ContainsIgnoreCase(this string text, string value)
        {
            return text.IndexOf(value, StringComparison.OrdinalIgnoreCase) != -1;
        }
    }
}
