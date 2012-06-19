using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Entities
{
    public class Identity
    {
        public string Key { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public string Provider { get; set; }

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