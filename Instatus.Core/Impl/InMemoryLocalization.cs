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
        private IPreferences preferences;
        private IHosting hosting;
        private static IDictionary<Tuple<string, string>, string> phrases = new ConcurrentDictionary<Tuple<string, string>, string>();
        
        public string Phrase(string key)
        {
            return phrases.GetValue(new Tuple<string, string>(preferences.Locale, key)) 
                ?? phrases.GetValue(new Tuple<string, string>(hosting.DefaultCulture.Name, key))
                ?? key.ToSeparatedWords();
        }

        public string Format(string key, params object[] values)
        {
            try
            {
                return string.Format(Phrase(key), values);
            }
            catch
            {
                return string.Join(" ", values);
            }
        }

        public static void Add(string locale, IDictionary<string, string> phrases) 
        {
            phrases
                .ToList()
                .ForEach(x => InMemoryLocalization.phrases[new Tuple<string, string>(locale, x.Key)] = x.Value);
        }

        public InMemoryLocalization(IPreferences preferences, IHosting hosting)
        {
            this.preferences = preferences;
            this.hosting = hosting;
        }
    }
}
