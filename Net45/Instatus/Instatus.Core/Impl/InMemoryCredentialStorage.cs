using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class InMemoryCredentialStorage : ICredentialStorage
    {
        private IDictionary<string, Credential> credentials;
        
        public Credential GetCredential(string providerName)
        {
            return credentials[providerName];
        }

        public InMemoryCredentialStorage(IDictionary<string, Credential> credentials)
        {
            this.credentials = credentials;
        }
    }
}
