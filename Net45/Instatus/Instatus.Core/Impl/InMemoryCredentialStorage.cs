using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryCredentialStorage : ICredentialStorage
    {
        private IDictionary<string, ICredential> credentials;
        
        public ICredential GetCredential(string providerName)
        {
            return credentials[providerName];
        }

        public InMemoryCredentialStorage(IDictionary<string, ICredential> credentials)
        {
            this.credentials = credentials;
        }
    }
}
