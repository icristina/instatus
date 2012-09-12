using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Instatus.Core.Impl
{
    public class InMemoryLocalization : ILocalization
    {
        private ISession session;
        
        private static IDictionary<Tuple<string, string>, string> allPhrases = new ConcurrentDictionary<Tuple<string, string>, string>();
        
        public const string DefaultLocale = "en-US";

        public string Phrase(string key)
        {
            return allPhrases[new Tuple<string, string>(session.Locale, key)] 
                ?? allPhrases[new Tuple<string, string>(DefaultLocale, key)];
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

        public static void Add(IDictionary<string, string> phrases)
        {
            Add(DefaultLocale, phrases);
        }

        public static void Add(string locale, IDictionary<string, string> phrases) 
        {
            phrases
                .ToList()
                .ForEach(x => allPhrases[new Tuple<string, string>(locale, x.Key)] = x.Value);
        }
        
        public InMemoryLocalization(ISession session)
        {
            this.session = session;
        }
    }
}
