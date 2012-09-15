using Instatus.Core;
using Instatus.Core.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Sample
{
    public class LocalizationConfig
    {
        public static void RegisterPhrases()
        {
            InMemoryLocalization.Add(new Dictionary<string, string>()
            {
                { WellKnown.Phrase.AppName, "Instatus" }
            });
        }
    }
}