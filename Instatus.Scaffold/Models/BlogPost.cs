using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Models
{
    public class BlogPost
    {
        public DateTime Published { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Picture { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}