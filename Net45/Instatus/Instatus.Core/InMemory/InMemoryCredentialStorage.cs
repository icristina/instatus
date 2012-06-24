using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.InMemory
{
    public class InMemoryCredentialStorage : ICredentialStorage
    {
        public class Credential : ICredential
        {
            public string ProviderName { get; set; }
            public string AccountName { get; set; }
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
            public DateTime ExpiryTime { get; set; }
            public string[] Claims { get; set; }
        }

        private IList<Credential> credentials;
        
        public ICredential GetCredential(string providerName)
        {
            return credentials.FirstOrDefault(c => c.ProviderName == providerName);
        }

        public InMemoryCredentialStorage(IList<Credential> credentials)
        {
            this.credentials = credentials;
        }
    }
}
