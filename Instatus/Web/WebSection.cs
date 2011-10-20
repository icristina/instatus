using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Instatus.Web
{
    public class WebSection : WebPart
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string Abstract { get; set; }
        public string Body { get; set; }
        public ICollection<WebLink> Links { get; set; }
        public ICollection<WebPart> Parts { get; set; }
        public ICollection<WebParameter> Parameters { get; set; }

        public WebSection()
        {
            Links = new List<WebLink>();
            Parts = new List<WebPart>();
            Parameters = new List<WebParameter>();
        }
    }
}