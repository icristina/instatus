using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;

namespace Instatus.Models
{
    public class Credential : Source
    {
        public string Claims { get; set; }
        public string AccessToken { get; set; }
        public string Secret { get; set; }
        public string Environment { get; set; }
        public string Features { get; set; }

        public virtual User User { get; set; }
        public int? UserId { get; set; }

        public virtual Application Application { get; set; }
        public int? ApplicationId { get; set; }

        public Credential() { }
        
        public Credential(WebProvider provider, object uri, string accessToken = null)
        {
            Provider = provider.ToString();
            Uri = uri.ToString();
            AccessToken = accessToken;
        }

        public bool HasFeature(string name)
        {
            var features = Features.ToList();
            return features.Any(f => string.Equals(f, name, StringComparison.OrdinalIgnoreCase));
        }

        public string ToUrn()
        {
            return string.Format("urn:{0}:{1}", Provider.ToLower(), Uri.ToLower());
        }

        public Credential WithEnvironment(WebEnvironment environment)
        {
            var credential = (Credential)this.MemberwiseClone();
            credential.Environment = environment.ToString();
            return credential;
        }
    }
}