using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryLocalization : ILocalization
    {
        private ISession session;
        
        // Tuple<locale, phraseKey>
        private static IDictionary<Tuple<string, string>, string> phrases = new ConcurrentDictionary<Tuple<string, string>, string>();
        public const string DefaultLocale = "en_US";

        public string Phrase(string key)
        {
            var localeKey = new Tuple<string, string>(session.Locale, key);
            var defaultLocaleKey = new Tuple<string, string>(DefaultLocale, key);
            
            return phrases[localeKey] ?? phrases[defaultLocaleKey];
        }

        public string Format(string key, params object[] values)
        {
            try
            {
                return string.Format(Phrase(key), values);
            }
            catch
            {
                return string.Join(", ", values);
            }
        }

        public InMemoryLocalization(ISession session)
        {
            this.session = session;
        }

        public static void AddOrUpdate(IDictionary<Tuple<string, string>, string> newPhrases) 
        {
            newPhrases.ToList().ForEach(x => phrases.Add(x.Key, x.Value));
        }
    }
}
