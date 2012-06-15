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
using System.Collections;
using Instatus.Data;

namespace Instatus
{
    public static class StringExtensions
    {
        public static string DefaultLocale = "en-GB";
        public static StringComparison DefaultComparison = StringComparison.OrdinalIgnoreCase;

        public static string AppendQueryParameter(this string uri, string name, object value)
        {
            if (value.IsEmpty())
                return uri;
            
            var seperator = uri.Contains('?') ? '&' : '?';

            if (value is string[] || value is int[])
                value = (value as IEnumerable).ToDelimited();

            return uri + seperator + name + '=' + HttpUtility.UrlEncode(value.AsString());
        }

        public static string AppendTimestampQueryParameter(this string uri, string name = "timestamp")
        {
            return uri.AppendQueryParameter(name, Generator.TimeStamp());
        }

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

        // http://stackoverflow.com/questions/244531/is-there-an-alternative-to-string-replace-that-is-case-insensitive
        public static string ReplaceString(this string str, string oldValue, string newValue, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);

                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }

            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }

        public static string RemoveHtmlElement(this string text, string element, bool preserveContent = true)
        {
            if (preserveContent)
                return text.RegexReplace("<[/]?" + element + "[^>]*>");

            return text
                    .RegexReplace("<" + element + "[^>]*/>") // self closing
                    .RegexReplace("<" + element + @"[^>]*>[.\s\S]*</" + element + "[^>]*>");
        }

        public static string RemoveHtmlElements(this string text, string[] elements, bool preserveContent = true)
        {
            return text.RemoveHtmlElement("(" + string.Join("|", elements)  + ")", preserveContent);
        }

        public static string RemoveHtmlEmphasis(this string text)
        {
            return text.RemoveHtmlElements(new string[] { "b", "em", "i" });
        }

        public static string RemoveHtmlFlowElements(this string text)
        {
            return text.RemoveHtmlElements(new string[] { "p", "div", "section", "header", "footer", "nav" });
        }

        public static string TidyHtml(this string text)
        {
            return text
                    .RemoveHtmlAttributes()
                    .RemoveHtmlElements(new string[] { "html", "head", "body", "span", "ins" })
                    .RemoveHtmlElements(new string[] { "script", "style", "meta", "link" }, false)
                    .RegexReplace("<p>&nbsp;</p>");
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

        public static string FindHtmlElement(this string text, string elementName)
        {
            var pattern = string.Format("<{0}[^<>]*>([^<>]+)</{0}>", elementName);
            return Regex.IsMatch(text, pattern) ? Regex.Match(text, pattern).Groups[1].Value.Trim() : string.Empty;
        }

        public static string RewriteRelativePaths(this string text)
        {
            return text.RegexReplace("~/[^\"]+", match => WebPath.Absolute(match));
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

        public static List<Tuple<string, string>> ToLabelledList(this string text, string regex = "<h2[^>]*>([^<]*)</h2>")
        {
            var result = new List<Tuple<string, string>>();
            
            if (text.IsEmpty())
                return result;
            
            var matches = Regex.Matches(text, regex, RegexOptions.IgnoreCase);

            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];

                if (i == 0 && match.Index > 0) // text before first boundary
                {
                    var precedingText = text.Substring(0, match.Index).TrimOrNull();
                    
                    if (!precedingText.IsEmpty()) {
                        result.Add(new Tuple<string, string>(string.Empty, precedingText));
                    }
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

        public static string Append(this string original, string value, bool condition = true)
        {
            return condition ? original + value : original;
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
            if (!title.IsEmpty())
            {
                sb.AppendLine();
                sb.AppendFormat("<section title=\"{0}\">", title);
                sb.AppendLine(body);
                sb.AppendLine("</section>");
            }
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

        public static MvcHtmlString AsRaw(this string text)
        {
            return new MvcHtmlString(text);
        }

        public static string ToUrlEncoded(this string text)
        {
            return HttpUtility.UrlEncode(text);
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

        // http://jclaes.blogspot.com/2012/01/autocorrecting-unknown-actions-using.html
        public static int LexicalDistance(this string str1, string str2) {
            var matrix = new int[str1.Length + 1, str2.Length + 1];

            for (var i = 0; i <= str1.Length; i++)
                matrix[i, 0] = i;
            for (var j = 0; j <= str2.Length; j++)
                matrix[0, j] = j;

            for (var i = 1; i <= str1.Length; i++)
            {
                for (var j = 1; j <= str2.Length; j++)
                {
                    var cost = str1[i - 1] == str2[j - 1] ? 0 : 1;

                    matrix[i, j] = (new[]
                    {
                        matrix[i - 1, j] + 1, matrix[i, j - 1] + 1, matrix[i - 1, j - 1] + cost
                    }).Min();

                    if ((i > 1) && 
                        (j > 1) && 
                        (str1[i - 1] == str2[j - 2]) &&
                        (str1[i - 2] == str2[j - 1]))
                    {
                        matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
                    }
                }
            }

            return matrix[str1.Length, str2.Length];
        }
    }
}