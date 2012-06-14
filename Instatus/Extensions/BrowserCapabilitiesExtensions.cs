using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus
{
    public static class BrowserCapabilitiesExtensions
    {
        public static string FlashWindowMode(this HttpBrowserCapabilitiesBase browser)
        {
            return browser == null || !browser.Browser.Match("firefox") ? "opaque" : "window";
        }
    }
}