using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using System.Runtime.Serialization;

namespace Instatus.Models
{
    [KnownType(typeof(Coupon))]
    public class Source
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public string Uri { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public string Data { get; set; }

        public virtual Page Page { get; set; }
        public int? PageId { get; set; }

        public Source() {
            CreatedTime = DateTime.UtcNow;
        }

        public Source(WebProvider provider, string uri) : this() 
        {
            Provider = provider.ToString();
            Uri = uri;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Provider, Uri);
        }
    }
}