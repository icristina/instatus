using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Objects;
using System.Text;
using System.Collections.Specialized;
using Instatus.Web;
using Instatus.Models;
using System.Collections;

namespace Instatus
{
    public static class CollectionExtensions
    {
        private static Random random = new Random();

        public static T Random<T>(this IEnumerable<T> list)
        {
            return list.ElementAt(random.Next(0, list.Count()));
        }

        public static ICollection<T> Random<T>(this IEnumerable<T> list, int min, int max)
        {
            var result = new List<T>();
            var count = random.Next(min, Math.Min(max, list.Count()));

            for (var i = 0; i < count; i++)
            {
                result.Add(list.Except(result).Random());
            }

            return result;
        }

        public static void ForFirst<T>(this IQueryable<T> list, Action<T> action)
        {
            if (list.Count() > 0)
                action(list.First());
        }

        public static T AddNonEmptyValues<T>(this T dictionary, T secondDictionary) where T : IDictionary<string, object>
        {
            foreach (var item in secondDictionary)
                dictionary.AddNonEmptyValue(item.Key, item.Value);

            return dictionary;
        }

        public static T AddNonEmptyValue<T, TValue>(this T dictionary, string name, TValue value) where T : IDictionary<string, object> {
            if (!value.IsEmpty() && !value.Equals(default(TValue)))
                if (dictionary.ContainsKey(name))
                    dictionary[name] = value;
                else
                    dictionary.Add(name, value);

            return dictionary;
        }

        public static T AddRequestParams<T>(this T dictionary) where T : IDictionary<string, object>
        {
            var nameValueCollection = HttpContext.Current.Request.Params;

            foreach (var param in nameValueCollection.AllKeys)
                dictionary.AddNonEmptyValue(param, nameValueCollection[param]);
            
            return dictionary;
        }

        public static IEnumerable<T> TryInclude<T>(this IEnumerable<T> source, string path)
        {
            var objectQuery = source as ObjectQuery<T>;
            if (objectQuery != null)
            {
                return objectQuery.Include(path);
            }
            return source;
        }

        // http://stackoverflow.com/questions/419019/split-list-into-sublists-with-linq
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }

        public static string ToSentence<T>(this IEnumerable<T> source)
        {
            var list = source.ToList();
            var count = source.Count();
            var sb = new StringBuilder();

            for (var i = 0; i < count; i++)
            {
                if (i > 0)
                {
                    sb.Append(i < count - 1 ? ", " : string.Format(" {0} ", WebPhrase.And));
                }

                sb.Append(list[i]);
            }

            return sb.ToString();
        }

        public static IQueryable<T> Expand<T>(this IQueryable<T> set, string[] paths) where T : class
        {
            IQueryable<T> queryable = set;

            if (!paths.IsEmpty())
                foreach (var path in paths)
                    queryable = queryable.Include(path);

            return queryable;
        }

        public static T Value<T>(this NameValueCollection collection, string name)
        {
            if (typeof(T).IsEnum)
                return (T)Enum.Parse(typeof(T), collection[name]);
            
            return (T)Convert.ChangeType(collection[name], typeof(T));
        }

        public static ICollection<T> Append<T>(this ICollection<T> set, IEnumerable<T> additions) {
            foreach (var item in additions)
                set.Add(item);

            return set;
        }

        public static IDbSet<T> Append<T>(this IDbSet<T> set, IEnumerable<T> additions) where T : class
        {
            foreach (var item in additions)
                set.Add(item);

            return set;
        }

        public static IEnumerable<T> ByRecency<T>(this IEnumerable<T> content) where T : IUserGeneratedContent
        {
            return content.OrderByDescending(c => c.CreatedTime);
        }

        public static IEnumerable<T> ByCreatedTime<T>(this IEnumerable<T> content) where T : IUserGeneratedContent
        {
            return content.OrderBy(c => c.CreatedTime);
        }

        public static List<string> ToStringList(this IEnumerable set)
        {
            var list = new List<string>();

            foreach (var item in set)
                list.Add(item.ToString());

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