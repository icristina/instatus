using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Part
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string Template { get; set; }
        public Zone Zone { get; set; }
        public string Scope { get; set; }
        public object RouteData { get; set; }
        public IList<Element> Elements { get; set; }
        public IList<Parameter> Parameters { get; set; }
        public IList<Part> Parts { get; set; }
        public IList<Link> Links { get; set; }
        public Query Query { get; set; }
        public Formatting Formatting { get; set; }

        public bool IsRawHtml
        {
            get
            {
                return Body.NonEmpty() && Template.IsEmpty() && RouteData.IsEmpty();
            }
        }
    }
}
