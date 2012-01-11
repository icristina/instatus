using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.Serialization;
using System.Globalization;
using System.Data.Entity.Design.PluralizationServices;
using System.Text.RegularExpressions;
using System.Web.Helpers;
using System.Text;
using System.Web.Mvc;
using Instatus.Web;
using System.Web.Security;

namespace Instatus
{
    public static class StringExtensions
    {
        public static string DefaultLocale = "en-GB";
        public static StringComparison DefaultComparison = StringComparison.OrdinalIgnoreCase;

        public static string ToLocalized(this string text)
        {
            return WebPhrase.Localize(text);
        }

        public static string ToSingular(this string text)
        {
            return PluralizationService.CreateService(new CultureInfo(DefaultLocale)).Singularize(text);
        }

        public static string ToPlural(this string text, int count = 0)
        {
            return count == 1 ? text : PluralizationService.CreateService(new CultureInfo(DefaultLocale)).Pluralize(text);
        }

        public static string ToCamelCase(this string text)
        {
            return string.IsNullOrEmpty(text) ? string.Empty : text.Substring(0, 1).ToLower() + text.Substring(1);
        }

        public static string ToPascalCase(this string text)
        {
            return string.IsNullOrEmpty(text) ? string.Empty : text.Substring(0, 1).ToUpper() + text.Substring(1);
        }

        public static string ToCapitalized(this string text)
        {
            return string.IsNullOrEmpty(text) ? string.Empty : text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
        }

        // http://stackoverflow.com/questions/155303/net-how-can-you-split-a-caps-delimited-string-into-an-array
        public static string ToCapitalizedDelimited(this string text)
        {
            return Regex.Replace(text, "(\\B[A-Z])", " $1"); // convert CapitalLetters to Capital Letters
        }

        public static string RegexReplace(this string text, string pattern, string replacement = "")
        {
            if (text == null || pattern == null)
                return text;

            return Regex.Replace(text, pattern, replacement, RegexOptions.IgnoreCase);
        }

        public static string RemoveDoubleSpaces(this string text)
        {           
            return text.RegexReplace(@"\s{1,}", " ");
        }

        public static string RemoveWhiteSpace(this string text)
        {
            return text.RegexReplace(@"\s+");
        }

        public static string RemoveHtml(this string text)
        {
            return text.RegexReplace("<[^<>]*>", "");
        }

        public static string RemoveHtmlElement(this string text, string element)
        {
            return text.RegexReplace("<[/]?" + element + "[^>]*>");
        }

        public static string RemoveHtmlEmphasis(this string text)
        {
            return text.RemoveHtmlElement("(b|em|i)");
        }

        public static string RemoveHtmlFlowElements(this string text)
        {
            return text.RemoveHtmlElement("(p|div|section|header|footer|nav)");
        }

        public static string ReplaceLineBreaksAsHtml(this string text)
        {
            return text.RegexReplace("\n", "<br/>");
        }

        // http://www.codinghorror.com/blog/2008/10/the-problem-with-urls.html
        // http://daringfireball.net/2010/07/improved_regex_for_matching_urls
        public static string ReplaceLinksAsHtml(this string text)
        {
            // simple: \bhttp://[^\s]+
            // orginal: \(?\bhttp://[-A-Za-z0-9+&@#/%?=~_()|!:,.;]*[-A-Za-z0-9+&@#/%=~_()|]
            // updated for https
            return text.RegexReplace(@"\(?\bhttps?://[-A-Za-z0-9+&@#/%?=~_()|!:,.;]*[-A-Za-z0-9+&@#/%=~_()|]", "<a href=\"$1\">$1<a>");
        }

        // http://regexpal.com/
        // http://tim.mackey.ie/CleanWordHTMLUsingRegularExpressions.aspx
        // (\s*\w+(:\w+)?=['"][^'"]*['"])
        public static string RemoveHtmlAttribute(this string text, string attribute)
        {
            return text.RegexReplace(@"(\s*" +  attribute + @"(:\w+)?=['""][^'""]*['""])");
        }

        public static string RemoveHtmlAttributes(this string text)
        {
            return text.RemoveHtmlAttribute(@"\w+");
        }

        public static string RemoveHtmlStyles(this string text)
        {
            return text.RemoveHtmlAttribute("(class|style|size|face)");
        }

        public static string RemoveSpecialCharacters(this string text)
        {
            return text.RegexReplace(@"[^a-z0-9\-\s]");
        }

        public static string ToEncrypted(this string text)
        {           
            return Crypto.Hash(text);
        }

        public static string ToEncrypted(this string text, string salt)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(text + salt, "md5");
        }

        public static string SubstringAfter(this string text, string match)
        {
            return text.Substring(text.LastIndexOf(match) + match.Length);
        }

        public static string SubstringBefore(this string text, string match)
        {
            return text.Substring(0, text.LastIndexOf(match));
        }

        public static bool Match(this string text, object match)
        {
            if (text.IsEmpty()) return false;
            return text.Equals(match.ToString(), DefaultComparison);
        }

        // &#8230; or &hellip;
        public static string MaxLength(this string text, int maxLength, bool elipsis = false)
        {
            if (text.IsEmpty()) return null;
            var suffix = elipsis ? "&hellip;" : "";
            return text.Length > maxLength ? text.Substring(0, maxLength) + suffix : text;
        }

        public static string ToSlug(this string text)
        {
            var slug = text
                            .ToLower()
                            .RemoveHtml()
                            .RemoveSpecialCharacters()
                            .RemoveDoubleSpaces()
                            .MaxLength(80)
                            .Trim();

            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"\-+", "-");
            
            return slug;
        }

        public static T AsEnum<T>(this string text) where T : struct
        {
            T value;

            if (Enum.TryParse<T>(text, true, out value))
                return value;
            
            return new T();
        }

        public static DateTime? AsDateTime(this string text)
        {
            DateTime dateTime;
            DateTime.TryParse(text, out dateTime);

            if(dateTime == DateTime.MinValue) return null;

            return dateTime;
        }

        public static List<string> ToList(this string text, char character = ',', bool toLowerCase = false, bool removeDuplicates = true)
        {
            var list = new List<string>();

            if (text.IsEmpty())
                return list;
            
            foreach(var item in text.Trim().Split(character)) {
                var trimmedItem = item.Trim();

                if (toLowerCase)
                    trimmedItem = trimmedItem.ToLower();

                if (!trimmedItem.IsEmpty() && !(removeDuplicates && list.Contains(trimmedItem)))
                    list.Add(trimmedItem);
            }

            return list;
        }

        public static List<Tuple<string, string>> ToLabelledList(this string text, string regex = "<h2[^>]*>([^<]*)</h2>")
        {
            var result = new List<Tuple<string, string>>();
            var matches = Regex.Matches(text, regex, RegexOptions.IgnoreCase);

            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];

                if (i == 0 && match.Index > 0) // text before first boundary
                {
                    result.Add(new Tuple<string, string>(string.Empty, text.Substring(0, match.Index)));
                }

                string label = string.Empty;
                string body = string.Empty;

                if (match.Groups.Count > 0)
                {
                    label = match.Groups[1].Value;
                }

                var boundaryEnd = match.Index + match.Length;

                if (i == matches.Count - 1) // last match
                {
                    body = text.Substring(boundaryEnd);
                }
                else
                {
                    body = text.Substring(boundaryEnd, matches[i + 1].Index - match.Index - match.Length);
                }

                result.Add(new Tuple<string, string>(label.TrimOrNull(), body.Trim()));
            }

            return result;
        }

        public static StringBuilder AppendDelimitedValue(this StringBuilder stringBuilder, object value, string delimiter = " ", bool unique = false)
        {
            var stringValue = value.ToString();
            
            if(unique) {
                var values = stringBuilder.ToString().Split(new string[] { delimiter }, StringSplitOptions.None);

                if(values.Contains(stringValue))
                    return stringBuilder;
            }

            if (stringBuilder.Length > 0)
                stringBuilder.Append(delimiter);

            return stringBuilder.Append(stringValue);
        }

        public static StringBuilder AppendSpace(this StringBuilder stringBuilder)
        {           
            return stringBuilder.Append(" ");
        }

        public static StringBuilder AppendSection(this StringBuilder sb, string title, string body)
        {
            sb.AppendLine();
            sb.AppendFormat("<section title=\"{0}\">", title);
            sb.AppendLine(body);
            sb.AppendLine("</section>");
            return sb;
        }

        public static string TrimOrNull(this string text, int minLength = 1)
        {
            if (text == null)
                return null;
            
            text = text.Trim();

            return text.Length < minLength ? null : text;
        }

        public static bool IsNumeric(this string text)
        {
            double num = 0;
            return Double.TryParse(text, out num);
        }

        public static bool IsAbsoluteUri(this string text)
        {
            return Uri.IsWellFormedUriString(text, UriKind.Absolute);
        }

        public static string ToHtmlEntites(this string text)
        {
            var sb = new StringBuilder();

            foreach (char character in text)
            {
                sb.AppendFormat("&#{0};", (int)character);
            }

            return sb.ToString();
        }
    }
}