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
using System.Linq.Expressions;

namespace Instatus
{
    public static class CollectionExtensions
    {
        private static Random random = new Random();

        public static T Random<T>(this IEnumerable<T> list)
        {
            return list.ElementAt(random.Next(0, list.Count()));
        }

        public static IEnumerable<T> Random<T>(this IEnumerable<T> list, int count) // fixed length, random result
        {
            if (list.Count() <= count)
                return list.Randomize();

            var result = new List<T>();

            for (var i = 0; i < count; i++)
            {
                result.Add(list.Except(result).Random());
            }

            return result;
        }

        public static IEnumerable<T> Random<T>(this IEnumerable<T> list, int min, int max) // random length, random result
        {
            var count = random.Next(min, Math.Min(max, list.Count()));
            return list.Random(count);
        }

        public static IList<T> Pad<T>(this IList<T> list, int count) where T : new()
        {
            var actualCount = list.Count();
            
            if (actualCount < count)
            {
                for (var i = 0; i < count - actualCount; i++)
                {
                    list.Add(new T());
                }
            }

            return list;
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(i => Guid.NewGuid());
        }

        public static bool AllEmpty<T1, T2>(this IDictionary<T1, T2> source)
        {
            return source.All(f => f.Value.IsEmpty());
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

        public static string ToDelimited(this IEnumerable source, string delimiter = ",")
        {
            return string.Join(delimiter, source.ToStringList());
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
            var type = typeof(T);
            var value = collection[name];
            
            if (type.IsEnum)
                return (T)Enum.Parse(type, value);

            if (type.IsValueType && value == null) // cannot convert null value type
                return default(T);

            return (T)Convert.ChangeType(value, type);
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

        public static IEnumerable<T> ByAlphabetical<T>(this IEnumerable<T> content) where T : INavigableContent
        {
            return content.OrderBy(c => c.Name);
        }

        public static IEnumerable<T> ByPriority<T>(this IEnumerable<T> content) where T : INavigableContent
        {
            return content.OrderBy(c => c.Priority);
        }

        public static IEnumerable<T> ByRecency<T>(this IEnumerable<T> content) where T : IUserGeneratedContent
        {
            return content.OrderByDescending(c => c.CreatedTime);
        }

        public static IEnumerable<T> ByCreatedTime<T>(this IEnumerable<T> content) where T : IUserGeneratedContent
        {
            return content.OrderBy(c => c.CreatedTime);
        }

        public static IOrderedEnumerable<T> AsOrdered<T>(this IEnumerable<T> set)
        {
            return set.OrderBy(a => true);
        }

        public static IEnumerable<WebEntry> DistinctByUri(this IEnumerable<WebEntry> resources)
        {
            return resources.Distinct(new WebEntryComparer());
        }

        public static List<string> ToStringList(this IEnumerable set)
        {
            var list = new List<string>();

            foreach (var item in set)
                list.Add(item.ToString());

            return list;
        }

        public static IEnumerable<T> RemoveNulls<T>(this IEnumerable<T> source)
        {
            return source.Where(item => item != null);
        }

        public static IEnumerable<T> RemoveNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source.Where(item => !item.IsEmpty());
        }

        public static ICollection<T> Synchronize<T>(this IEnumerable<T> source, Func<T, T> existingMatch, bool excludeNonMatches = false) where T : class
        {
            if (source.IsEmpty())
                return null;

            return source.Select(item => existingMatch(item) ?? (excludeNonMatches ? null : item))
                    .RemoveNulls()    
                    .ToList();
        }

        public static string ToPositionString<T>(this IEnumerable<T> source, T item, string first = "first", string last = "last") where T : class
        {
            if(source.First() == item)
                return first;

            if(source.Last() == item)
                return last;

            return string.Empty;
        }

        public static void ReplaceOrAdd<T>(this IList<T> source, T original, T replacement)
        {
            if (original.IsEmpty() || !source.Contains(original))
            {
                source.Add(replacement);
            }
            else
            {
                var index = source.IndexOf(original);
                source.Remove(original);
                source.Insert(index, replacement);
            }
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