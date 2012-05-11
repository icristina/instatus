using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Models;

namespace Instatus.Entities
{
    public class Credential
    {
        public int Id { get; set; }

        public string Key { get; set; }
        public string Secret { get; set; }
        
        public string Alias { get; set; }

        public Application Application { get; set; }
        public int ApplicationId { get; set; }
        
        public string Scope { get; set; }
        public string Features { get; set; }
#if NET45
        public Deployment Deployment { get; set; }
        public Provider Provider { get; set; }
#else
        public string Deployment { get; set; }
        public string Provider { get; set; }
#endif

        public bool HasFeature(string name)
        {
            return Features.ToList().Any(f => f.Match(name));
        }

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
