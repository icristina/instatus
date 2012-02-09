using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebFeed
    {
        public string Title { get; set; }
        public IList<WebEntry> Entries { get; set; }

        public WebFeed(string title)
            : this()
        {
            Title = title;
        }

        public WebFeed()
        {
            Entries = new List<WebEntry>();
        }
    }
}