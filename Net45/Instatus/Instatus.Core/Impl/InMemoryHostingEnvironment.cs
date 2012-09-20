using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryHostingEnvironment : IHostingEnvironment
    {
        private IDictionary<string, string> appSettings;

        public string RootPath { get; set; }
        public string BaseAddress { get; set; }
        public string ServerName { get; set; }

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
