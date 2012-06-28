using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryHostingEnvironment : IHostingEnvironment
    {
        private string baseUri;
        private string loginUrl;
        private IDictionary<string, string> appSettings;
        
        public string BaseUri
        {
            get 
            { 
                return baseUri; 
            }
        }

        public string LoginUrl
        {
            get 
            { 
                return loginUrl; 
            }
        }

        public string GetAppSetting(string key)
        {
            return appSettings[key];
        }

        public InMemoryHostingEnvironment(string baseUri, string loginUrl, IDictionary<string, string> appSettings)
        {
            this.baseUri = baseUri;
            this.loginUrl = loginUrl;
            this.appSettings = appSettings;
        }
    }
}
