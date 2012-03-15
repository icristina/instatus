using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus
{
    public static class TypeExtensions
    {
        public static bool Implements<TInterface, T>()
        {
            return typeof(TInterface).IsAssignableFrom(typeof(T));
        }

        public static bool HasAttribute<TAttribute, T>() 
        {
            return typeof(T).GetCustomAttributes(typeof(TAttribute), true).OfType<TAttribute>().Any();
        }
    }
}