using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public abstract class ExtendedViewPage : WebViewPage
    {
        public ILocalization Localization { get; set; }
        public ISessionData SessionData { get; set; }

        public string Phrase(string key)
        {
            return Localization.Phrase(key);
        }

        public string Format(string key, params object[] values)
        {
            return Localization.Format(key, values);
        }
    }
}
