using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Models;
using Instatus;

namespace Instatus.Entities
{
    public class Credential : ICredential
    {
        public int Id { get; set; }

        public string Key { get; set; }
        public string Secret { get; set; }
        
        public string Alias { get; set; }

        public Application Application { get; set; }
        public int ApplicationId { get; set; }
        
        public string Scope { get; set; }
        public string Features { get; set; }
        public string Deployment { get; set; }
        public string Provider { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Provider, Key, Deployment);
        }

        public Credential() 
        {
            Deployment = Instatus.Models.Deployment.All.ToString();
        }

        public Credential(Provider provider, Deployment deployment = Instatus.Models.Deployment.All) : this()
        {
            Provider = provider.ToString();
            Deployment = deployment.ToString();
        }
    }
}

namespace Instatus
{
    public static class CredentialExtensions
    {
        public static bool HasFeature(this ICredential credential, string name)
        {
            return credential.Features.ToList().Any(f => f.ToString().Match(name));
        }
    }
}
