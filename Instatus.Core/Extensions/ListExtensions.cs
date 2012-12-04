using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Extensions
{
    public static class ListExtensions
    {
        private static Random random = new Random();
        
        public static T Random<T>(this IList<T> list)
        {
            return list.ElementAt(random.Next(0, list.Count()));
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
    }
}
