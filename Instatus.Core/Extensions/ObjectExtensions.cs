using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static T ThrowIfNull<T>(this T obj, string message)
        {
            if (obj == null)
                throw new Exception(message);

            return obj;
        }

        public static string AsString(this object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }
    }
}
