﻿using System;
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
        private ISessionData sessionData;
        
        private static IDictionary<Tuple<string, string>, string> allPhrases = new ConcurrentDictionary<Tuple<string, string>, string>();
        private static List<string> locales = new List<string>();
        
        public string Phrase(string key)
        {
            return allPhrases.GetValue(new Tuple<string, string>(sessionData.Locale, key)) 
                ?? allPhrases.GetValue(new Tuple<string, string>(WellKnown.Locale.UnitedStates, key))
                ?? key;
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
            Add(WellKnown.Locale.UnitedStates, phrases);
        }

        public static void Add(string locale, IDictionary<string, string> phrases) 
        {
            if (!locales.Contains(locale))
                locales.Add(locale);
            
            phrases
                .ToList()
                .ForEach(x => allPhrases[new Tuple<string, string>(locale, x.Key)] = x.Value);
        }

        public static string[] GetLocales()
        {
            return locales.ToArray();
        }

        public static string[] GetEnglishLocaleNames()
        {
            return locales.Select(l => new CultureInfo(l).EnglishName).ToArray();
        }
        
        public InMemoryLocalization(ISessionData sessionData)
        {
            this.sessionData = sessionData;
        }
    }
}
