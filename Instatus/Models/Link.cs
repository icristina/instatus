using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Link
    {
        public string Uri { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Rel { get; set; }
        public string ContentType { get; set; }
        public string Picture { get; set; }
    }
}
