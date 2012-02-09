using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public interface IWebFeed
    {
        string Title { get; }
        IEnumerable<WebEntry> Entries { get; } 
    }
    
    public class WebFeed : IWebFeed
    {
        public string Title { get; set; }
        public IEnumerable<WebEntry> Entries { get; set; }

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

    public class DeferredWebFeed : IWebFeed
    {
        private IEnumerable<ISyndicatable> items;

        public string Title { get; set; }

        public IEnumerable<WebEntry> Entries
        {
            get
            {
                return items.Select(e => e.ToWebEntry());
            }
        }

        public DeferredWebFeed(IEnumerable<ISyndicatable> items)
        {
            this.items = items;
        }
    }
}