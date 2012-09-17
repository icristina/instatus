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
    }
}
