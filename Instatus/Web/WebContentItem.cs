using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebContentItem : IContentItem
    {
        public string Name { get; set; }
        public WebDocument Document { get; set; }
        public IDictionary<WebVerb, IWebFeed> Feeds { get; set; }
        public dynamic Extensions { get; set; }

        public WebContentItem()
        {
            Document = new WebDocument();
            Feeds = new Dictionary<WebVerb, IWebFeed>();
            Extensions = new ExpandoObject();
        }
    }
}