using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using System.Dynamic;
using System.Runtime.Serialization;

namespace Instatus.Web
{
    public class WebDocument
    {       
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }

        public ICollection<WebPart> Parts { get; set; }
        public ICollection<WebLink> Links { get; set; }
        
        public WebMetadata Metadata { get; set; }

        public WebDocument()
        {
            Parts = new List<WebPart>();
            Links = new List<WebLink>();
        }
    }
}