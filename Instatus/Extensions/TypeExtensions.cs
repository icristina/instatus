using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static bool HasAttribute<TAttribute>(this MemberInfo property)
        {
            return property.GetCustomAttributes(typeof(TAttribute), true).OfType<TAttribute>().Any();
        }

        public static string GetNamespaceByConvention(this Type type, string conventionName = "Areas")
        {
            var namespaces = type.Namespace.Split('.').ToList();
            var indexOfAreas = namespaces.IndexOf(conventionName);

            if (indexOfAreas > 0 && indexOfAreas < namespaces.Count - 1)
            {
                return namespaces[indexOfAreas + 1];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}