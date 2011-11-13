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

        public static string RemoveDoubleSpaces(this string text)
        {
            return Regex.Replace(text, @"\s{2,}", " ").Trim();
        }

        public static string RemoveHtml(this string text)
        {
            return Regex.Replace(text, "<[^<>]*>", "");
        }

        public static string RemoveSpecialCharacters(this string text)
        {
            return Regex.Replace(text, @"[^a-z0-9\-\s]", "");
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
            return text.Equals(match.ToString(), DefaultComparison);
        }

        public static string MaxLength(this string text, int maxLength, bool elipsis = false)
        {
            var suffix = elipsis ? "&elip;" : "";
            return text.Length > maxLength ? text.Substring(0, maxLength - 1) + suffix : text;
        }

        public static string ToSlug(this string text)
        {
            var slug = text
                            .ToLower()
                            .RemoveDoubleSpaces()
                            .RemoveHtml()
                            .RemoveSpecialCharacters()
                            .MaxLength(80)
                            .Trim();

            slug = Regex.Replace(slug, @"\s+", "-");
            
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

        public static string TrimOrNull(this string text, int minLength = 1)
        {
            if (text == null)
                return null;
            
            text = text.Trim();

            return text.Length < minLength ? null : text;
        }
    }
}