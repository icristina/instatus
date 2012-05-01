using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Instatus.Entities
{
    [ComplexType]
    public class Identity
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string AccessToken { get; set; }
#if NET45
        public Provider Provider { get; set; }
#else
        public string Provider { get; set; }
#endif

        public string ToUrn()
        {
            return string.Format("urn:{0}:{1}", Provider.ToLower(), UserId.ToLower());
        }
    }
}