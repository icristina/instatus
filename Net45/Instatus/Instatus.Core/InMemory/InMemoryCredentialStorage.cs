using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.InMemory
{
    public class InMemoryCredentialStorage : ICredentialStorage
    {
        internal class Credential : ICredential
        {
            public string ProviderName { get; set; }
            public string UserName { get; set; }
            public string AccountName { get; set; }
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
            public DateTime ExpiryTime { get; set; }
            public string[] Claims { get; set; }
        }

        private IList<Credential> credentials = new List<Credential>();
        
        public ICredential GetCredential(string providerName)
        {
            return credentials.FirstOrDefault(c => string.IsNullOrEmpty(c.UserName) && c.ProviderName == providerName);
        }

        public ICredential GetCredential(string providerName, string userName)
        {
            return credentials.FirstOrDefault(c => c.ProviderName == providerName && c.UserName == userName);
        }

        public void SaveCredential(string providerName, string userName, ICredential credential)
        {
            credentials.Add(new Credential()
            {
                ProviderName = providerName,
                UserName = userName,
                AccountName = credential.AccountName,
                PublicKey = credential.PublicKey,
                PrivateKey = credential.PrivateKey,
                ExpiryTime = credential.ExpiryTime,
                Claims = credential.Claims
            });
        }
    }
}
