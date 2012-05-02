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
        public string ApplicationId { get; set; }
        public string ApplicationAlias { get; set; }
        public string ApplicationSecret { get; set; }
        public Application Application { get; set; }
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
