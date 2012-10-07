using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryHosting : IHosting
    {
        private IDictionary<string, string> appSettings;

        public string RootPath { get; set; }
        public string BaseAddress { get; set; }
        public string ServerName { get; set; }

        public string GetAppSetting(string key)
        {
            return appSettings[key];
        }

        public CultureInfo DefaultCulture { get; set; }
        public CultureInfo[] SupportedCultures { get; set; }

        public InMemoryHosting(IDictionary<string, string> appSettings)
        {
            this.appSettings = appSettings;
        }
    }
}
