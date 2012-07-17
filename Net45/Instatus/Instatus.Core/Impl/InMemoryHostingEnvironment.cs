using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryHostingEnvironment : IHostingEnvironment
    {
        private IDictionary<string, string> appSettings;

        public string OutputPath { get; set; }
        public string BaseUri { get; set; }
        public string LoginUrl { get; set; }

        public string GetAppSetting(string key)
        {
            return appSettings[key];
        }

        public InMemoryHostingEnvironment(IDictionary<string, string> appSettings)
        {
            this.appSettings = appSettings;
        }
    }
}
