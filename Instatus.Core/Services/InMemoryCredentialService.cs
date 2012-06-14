using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Entities;
using Instatus.Models;

namespace Instatus.Services
{
    public class InMemoryCredentialService : ICredentialService
    {
        public static IDictionary<Provider, ICredential> Credentials = new Dictionary<Provider, ICredential>();
        
        public ICredential GetCredential(Provider provider)
        {
            return Credentials[provider];
        }

        public InMemoryCredentialService() { }

        public InMemoryCredentialService(Provider provider, ICredential credential)
        {
            Credentials.Add(provider, credential);
        }
    }
}
