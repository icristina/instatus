using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Instatus.Models;

namespace Instatus
{
    public static class CommonExtensions
    {
        public static StringComparison DefaultComparison = StringComparison.OrdinalIgnoreCase;
        
        public static bool Match(this string text, object match)
        {
            if (string.IsNullOrWhiteSpace(text) || match == null) return false;
            return text.Equals(match.ToString(), DefaultComparison);
        }

        public static Stream ToStream(this string text)
        {
            var stream = new MemoryStream(Encoding.Default.GetBytes(text));
            return stream.ResetPosition();
        }

        public static string CopyToString(this Stream stream)
        {
            var input = new StreamReader(stream.ResetPosition()).ReadToEnd();
            stream.ResetPosition();
            return input;
        }

        public static Stream ResetPosition(this Stream stream)
        {
            stream.Position = 0;
            return stream;
        }

        public static bool IsEncrypted(this string text)
        {
            return !string.IsNullOrWhiteSpace(text) && Regex.IsMatch(text, "^[A-Z0-9]{64}$"); // sha256 64 characters alphanumeric
        }

        // http://aspnetwebstack.codeplex.com/SourceControl/changeset/view/ecc0b5f2ea9a#src%2fSystem.Web.Helpers%2fCrypto.cs
        public static string ToEncrypted(this string text)
        {
            //return Crypto.Hash(text);
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(text);
                return sha256.ComputeHash(bytes).ToHex();
            }
        }

        public static string ToHex(this byte[] data)
        {
            char[] hex = new char[data.Length * 2];

            for (int iter = 0; iter < data.Length; iter++)
            {
                byte hexChar = ((byte)(data[iter] >> 4));
                hex[iter * 2] = (char)(hexChar > 9 ? hexChar + 0x37 : hexChar + 0x30);
                hexChar = ((byte)(data[iter] & 0xF));
                hex[(iter * 2) + 1] = (char)(hexChar > 9 ? hexChar + 0x37 : hexChar + 0x30);
            }
            return new string(hex);
        }

        public static string RegexReplace(this string text, string pattern, string replacement = "")
        {
            if (text == null || pattern == null)
                return text;

            return Regex.Replace(text, pattern, replacement, RegexOptions.IgnoreCase);
        }

        public static string RegexReplace(this string text, string pattern, Func<string, string> replacement)
        {
            if (text == null || pattern == null)
                return text;

            return Regex.Replace(text, pattern, delegate(Match m)
            {
                return replacement(m.Value);
            },
            RegexOptions.IgnoreCase);
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

        public static string RemoveSpecialCharacters(this string text, bool allowDashes = true)
        {
            return allowDashes ? text.RegexReplace(@"[^a-z0-9\-\s]") : text.RegexReplace(@"[^a-z0-9\s]");
        }

        // &#8230; or &hellip;
        public static string MaxLength(this string text, int maxLength, bool elipsis = false)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
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

        public static bool IsEmpty(this object graph)
        {
            return graph == null
                || (graph is IHasValue && !((IHasValue)graph).HasValue)
                || (graph is string && string.IsNullOrWhiteSpace((string)graph))
                || (graph is ICollection && ((ICollection)graph).Count == 0)
                || (graph is IEnumerable && Count(((IEnumerable)graph)) == 0)
                || (graph is DateTime && (DateTime)graph == DateTime.MinValue);
        }

        public static bool AllEmpty<T1, T2>(this IDictionary<T1, T2> source)
        {
            return source.All(f => f.Value.IsEmpty());
        }

        public static byte[] Serialize<T>(this T graph, IEnumerable<Type> knownTypes = null)
        {
            var stream = new MemoryStream();
            var serializer = new DataContractSerializer(typeof(T), knownTypes);
            serializer.WriteObject(stream, graph);
            return stream.ToArray();
        }

        public static T Deserialize<T>(this byte[] bytes, IEnumerable<Type> knownTypes = null)
        {
            if (bytes == null)
                return default(T);

            MemoryStream stream = new MemoryStream(bytes);
            stream.Position = 0;
            var serializer = new DataContractSerializer(typeof(T), knownTypes);
            return (T)serializer.ReadObject(stream);
        }

        public static List<string> ToList(this string text, char character = ',', bool toLowerCase = false, bool removeDuplicates = true)
        {
            var list = new List<string>();

            if (text.IsEmpty())
                return list;

            foreach (var item in text.Trim().Split(character))
            {
                var trimmedItem = item.Trim();

                if (toLowerCase)
                    trimmedItem = trimmedItem.ToLower();

                if (!trimmedItem.IsEmpty() && !(removeDuplicates && list.Contains(trimmedItem)))
                    list.Add(trimmedItem);
            }

            return list;
        }

        // http://msmvps.com/blogs/matthieu/archive/2009/04/01/how-to-use-linq-extension-methods-on-non-generic-ienumerable.aspx
        public static int Count(IEnumerable source)
        {
            int count = 0;
            foreach (var item in source)
                count++;
            return count;
        }

        public static object First(IEnumerable source)
        {
            var enumerator = source.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current;
        }
    }
}
