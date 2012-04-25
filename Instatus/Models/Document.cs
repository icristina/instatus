using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Document
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public IList<Part> Parts { get; set; }
        public IList<Parameter> Parameters { get; set; }
        public IList<Link> Links { get; set; }
    }
}
