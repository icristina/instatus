using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus
{
    public static class TypeExtensions
    {
        public static bool Implements<T1, T2>()
        {
            return typeof(T1).IsAssignableFrom(typeof(T2));
        }
    }
}