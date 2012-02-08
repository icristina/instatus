using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Instatus.Web
{
    [KnownType(typeof(WebMedia))]
    public class WebLink
    {
        [Key]
        public string Uri { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Rel { get; set; }
        public string ContentType { get; set; }
        public string Picture { get; set; }

        public override string ToString()
        {
            return Title ?? Description ?? Uri;
        }
    }
}