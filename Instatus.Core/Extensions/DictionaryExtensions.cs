using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static T GetValue<TKey, T>(this IDictionary<TKey, T> dictionary, TKey key)
        {
            T output;

            if (dictionary.TryGetValue(key, out output))
            {
                return output;
            }

            return default(T);
        }        
        
        public static T GetValue<T>(this IDictionary<string, object> dictionary, string key)
        {
            object output;

            if (dictionary.TryGetValue(key, out output))
            {
                try
                {
                    return (T)Convert.ChangeType(output, typeof(T));
                }
                catch
                {

                }
            }

            return default(T);
        }

        public static T GetValue<T>(this IDictionary<string, object> dictionary, string key, Func<T> activator)
        {
            var value = dictionary.GetValue<T>(key);

            if (value == null)
            {
                value = activator();
                dictionary[key] = value;
            }
            
            return value;
        }
    }
}
