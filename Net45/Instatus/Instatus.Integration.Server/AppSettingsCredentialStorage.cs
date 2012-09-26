using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core;
using Instatus.Core.Extensions;
using Instatus.Core.Models;

namespace Instatus.Integration.Server
{
    public class AppSettingsCredentialStorage : ICredentialStorage
    {
        private IHostingEnvironment hostingEnvironment;
        private ConcurrentDictionary<string, Credential> credentials = new ConcurrentDictionary<string, Credential>();

        private Credential ConvertToCredential(IDictionary<string, object> values)
        {
            return new Credential()
            {
                AccountName = values.GetValue<string>("AccountName"),
                PrivateKey = values.GetValue<string>("PrivateKey"),
                PublicKey = values.GetValue<string>("PublicKey"),
                Claims = (values.GetValue<string>("Claims") ?? "").Split(',')
            };
        }

        public Credential GetCredential(string providerName)
        {
            Credential credential;
            
            if (!credentials.TryGetValue(providerName, out credential)) 
            {
                var setting = hostingEnvironment.GetAppSetting("Credential".WithNamespace(providerName));

                if (setting == null)
                    return null;

                credential = ConvertToCredential(setting.AsDictionary());
                credentials.TryAdd(providerName, credential);                
            }
            
            return credential;
        }

        public AppSettingsCredentialStorage(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
    }
}
