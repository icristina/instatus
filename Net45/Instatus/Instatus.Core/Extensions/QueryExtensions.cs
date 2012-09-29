using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Extensions
{
    public static class QueryExtensions
    {
        public static IOrderedQueryable<T> ByRecency<T>(this IQueryable<T> list) where T : ICreated
        {
            return list.OrderByDescending(t => t.Created);
        }

        public static IOrderedQueryable<T> AsOrderedQueryable<T>(this IEnumerable<T> list)
        {
            return list as IOrderedQueryable<T> ?? list.AsQueryable().OrderBy(l => true);
        }
    }
}
