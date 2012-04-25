using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Deployment Environment { get; set; }
        public Provider Provider { get; set; }
#else
        public string Environment { get; set; }
        public string Provider { get; set; }
#endif

        public bool HasFeature(string name)
        {
            return Features.ToList().Any(f => f.Match(name));
        }
    }
}
