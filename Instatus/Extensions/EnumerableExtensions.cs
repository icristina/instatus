﻿using System;
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
using Instatus.Data;

namespace Instatus
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Paging<T>(this IEnumerable<T> list, int pageIndex, int pageSize)
        {
            return list.AsOrdered().Skip(pageIndex * pageSize).Take(pageSize);
        }
        
        private static Random random = new Random();

        public static T Random<T>(this IList<T> list)
        {
            return list.ElementAt(random.Next(0, list.Count()));
        }

        public static IEnumerable<T> Random<T>(this IList<T> list, int count) // fixed length, random result
        {
            if (list.Count() <= count)
                return list.Randomize();

            var result = new List<T>();

            for (var i = 0; i < count; i++)
            {
                result.Add(list.Except(result).ToList().Random());
            }

            return result;
        }

        public static IEnumerable<T> Random<T>(this IList<T> list, int min, int max) // random length, random result
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

        public static IList<T> Pop<T>(this IList<T> source)
        {
            if (source.Count() > 0)
                source.RemoveAt(source.Count - 1);

            return source;
        }

        public static IList<T> Randomize<T>(this IList<T> source)
        {
            return source
                .OrderBy(i => Guid.NewGuid())
                .ToList();
        }

        public static T2 Value<T1, T2>(this IDictionary<T1, T2> source, T1 key)
        {
            T2 value;
            source.TryGetValue(key, out value);
            return value;
        }

        public static bool AllEmpty<T1, T2>(this IDictionary<T1, T2> source)
        {
            return source.All(f => f.Value.IsEmpty());
        }

        public static void ForFirst<T>(this IEnumerable<T> list, Action<T> action)
        {
            if (!list.IsEmpty())
                action(list.First());
        }

        public static T AddNonEmptyValues<T>(this T dictionary, T secondDictionary) where T : IDictionary<string, object>
        {
            foreach (var item in secondDictionary)
                dictionary.AddNonEmptyValue(item.Key, item.Value);

            return dictionary;
        }

        public static IDictionary<TKey, TValue> AddNonEmptyValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey name, TValue value)
        {
            if (!value.IsEmpty() && !value.Equals(default(TValue)))
                if (dictionary.ContainsKey(name))
                    dictionary[name] = value;
                else
                    dictionary.Add(name, value);

            return dictionary;
        }

        private const string dictionaryPrefix = "__SingleInstance.";

        public static IDictionary<string, object> AddSingle<T>(this IDictionary<string, object> dictionary, T item) where T : class
        {
            if (dictionary != null && item != null)
            {
                dictionary.Add(dictionaryPrefix + item.GetType().FullName, item);
            }
            
            return dictionary;
        }

        public static T GetSingle<T>(this IDictionary<string, object> dictionary) where T : class
        {
            return dictionary[dictionaryPrefix + typeof(T).FullName] as T ?? Activator.CreateInstance<T>();
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

        public static IEnumerable<T> Prepend<T>(this ICollection<T> set, T addition)
        {
            var newList = new List<T>()
            {
                addition
            }.Concat(set);

            return newList;
        }

        public static ICollection<T> Append<T>(this ICollection<T> set, T addition)
        {
            set.Add(addition);
            return set;
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

        public static IEnumerable<T> ByRecency<T>(this IEnumerable<T> content) where T : ITimestamp
        {
            return content.OrderByDescending(c => c.CreatedTime);
        }

        public static IEnumerable<T> ByCreatedTime<T>(this IEnumerable<T> content) where T : ITimestamp
        {
            return content.OrderBy(c => c.CreatedTime);
        }

        public static IEnumerable<T> FilterByRules<T>(this IEnumerable<T> content, IEnumerable<IRule<T>> rules)
        {
            return content.Where(c => rules.All(rule => rule.Evaluate(c)));
        }

        public static T FirstByName<T>(this IEnumerable<T> content, string name) where T : INamed
        {
            return content.FirstOrDefault(n => n.Name.Match(name));
        }

        public static IEnumerable<T> AsOrdered<T>(this IEnumerable<T> list)
        {
            if (list is IOrderedQueryable<T> || list is IOrderedEnumerable<T>)
                return list;
            
            return list.OrderBy(a => true);
        }

        public static IEnumerable<WebEntry> DistinctByUri(this IEnumerable<WebEntry> resources)
        {
            return resources.Distinct(new ResourceComparer<WebEntry>());
        }

        public static List<string> ToStringList(this IEnumerable set)
        {
            if (set is List<string>)
                return (List<string>)set;
            
            var list = new List<string>();

            if (set.IsEmpty())
                return list;

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

        public static void RemoveAll<TItem, T>(this ICollection<T> source) where TItem : T
        {
            foreach (var item in source.OfType<TItem>().ToList())
            {
                source.Remove(item);
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