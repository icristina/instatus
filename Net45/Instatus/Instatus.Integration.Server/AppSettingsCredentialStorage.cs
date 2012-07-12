using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core;

namespace Instatus.Integration.Server
{
    public class AppSettingsCredentialStorage : ICredentialStorage
    {
        private IHostingEnvironment hostingEnvironment;
        private ConcurrentDictionary<string, ICredential> credentials = new ConcurrentDictionary<string, ICredential>();

        public IDictionary<string, string> ParseDelimitedString(string input)
        {
            var values = new Dictionary<string, string>();
            var segments = input.Split(';');

            foreach (var setting in segments.Where(s => s.Length >= 3))
            {
                var startIndex = setting.IndexOf('=');
                var key = setting.Substring(0, startIndex);
                var value = setting.Substring(startIndex + 1);
                
                values.Add(key.Trim(), value.Trim());
            }

            return values;
        }

        public ICredential ConvertToCredential(IDictionary<string, string> values)
        {
            return new AppSettingsCredential()
            {
                AccountName = GetValue<string>(values, "AccountName"),
                PrivateKey = GetValue<string>(values, "PrivateKey"),
                PublicKey = GetValue<string>(values, "PublicKey")
            };
        }

        public T GetValue<T>(IDictionary<string, string> dictionary, string key)
        {
            string output;

            if (dictionary.TryGetValue(key, out output))
                return (T)Convert.ChangeType(output, typeof(T));

            return default(T);
        }

        public string GetAppSettingKey(string providerName) 
        {
            return providerName + ".Credential";
        }

        public ICredential GetCredential(string providerName)
        {
            ICredential credential;
            
            if (!credentials.TryGetValue(providerName, out credential)) 
            {
                var key = GetAppSettingKey(providerName);
                var setting = hostingEnvironment.GetAppSetting(key);

                if (setting == null)
                    return null;

                var dictionary = ParseDelimitedString(setting);
                
                credential = ConvertToCredential(dictionary);
                credentials.TryAdd(providerName, credential);                
            }
            
            return credential;
        }

        public AppSettingsCredentialStorage(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public class AppSettingsCredential : ICredential
        {
            public string AccountName { get; set; }
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
            public string[] Claims { get; set; }
        }
    }
}
