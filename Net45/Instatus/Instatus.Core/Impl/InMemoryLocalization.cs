using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Instatus.Core.Extensions;
using System.Globalization;

namespace Instatus.Core.Impl
{
    public class InMemoryLocalization : ILocalization
    {
        private static IDictionary<Tuple<string, string>, string> phrases = new ConcurrentDictionary<Tuple<string, string>, string>();
        private static List<CultureInfo> supportedCultures = new List<CultureInfo>();
        
        public string Phrase(string locale, string key)
        {
            return phrases.GetValue(new Tuple<string, string>(locale, key)) 
                ?? phrases.GetValue(new Tuple<string, string>(WellKnown.Locale.UnitedStates, key))
                ?? key;
        }

        public string Format(string locale, string key, params object[] values)
        {
            try
            {
                return string.Format(Phrase(locale, key), values);
            }
            catch
            {
                return string.Join(" ", values);
            }
        }

        public CultureInfo[] SupportedCultures
        {
            get 
            {
                return supportedCultures.ToArray();
            }
        }

        public static void Add(IDictionary<string, string> phrases)
        {
            Add(WellKnown.Locale.UnitedStates, phrases);
        }

        public static void Add(string locale, IDictionary<string, string> phrases) 
        {
            if (!supportedCultures.Any(c => c.Name.Equals(locale)))
                supportedCultures.Add(CultureInfo.CreateSpecificCulture(locale));
            
            phrases
                .ToList()
                .ForEach(x => InMemoryLocalization.phrases[new Tuple<string, string>(locale, x.Key)] = x.Value);
        }
    }
}
