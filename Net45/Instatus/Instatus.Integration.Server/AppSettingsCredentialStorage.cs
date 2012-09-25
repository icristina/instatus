﻿using System;
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

        public IDictionary<string, object> ParseDelimitedString(string input)
        {
            var values = new Dictionary<string, object>();
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

        public Credential ConvertToCredential(IDictionary<string, object> values)
        {
            return new Credential()
            {
                AccountName = values.GetValue<string>("AccountName"),
                PrivateKey = values.GetValue<string>("PrivateKey"),
                PublicKey = values.GetValue<string>("PublicKey"),
                Claims = (values.GetValue<string>("Claims") ?? "").Split(',')
            };
        }

        public string GetAppSettingKey(string providerName) 
        {
            return providerName + ".Credential";
        }

        public Credential GetCredential(string providerName)
        {
            Credential credential;
            
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
    }
}
