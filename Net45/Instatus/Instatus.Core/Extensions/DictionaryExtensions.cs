using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static T GetValue<T>(this IDictionary<string, object> dictionary, string key)
        {
            object output;

            if (dictionary.TryGetValue(key, out output))
                return (T)Convert.ChangeType(output, typeof(T));

            return default(T);
        }
    }
}
