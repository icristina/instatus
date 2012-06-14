using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Entities
{
    [ComplexType]
    public class Identity
    {
        public string Key { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public DateTime? ExpiryTime { get; set; }
#if NET45
        public Provider Provider { get; set; }
#else
        public string Provider { get; set; }
#endif

        public override string ToString()
        {
            return string.Format("{0}:{1}", Provider.ToLower(), Key.ToLower());
        }

        public Identity() { }

        public Identity(Provider provider)
        {
            Provider = provider.ToString();
        }
    }
}