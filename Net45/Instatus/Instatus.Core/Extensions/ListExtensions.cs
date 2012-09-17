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
    }
}
