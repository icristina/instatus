using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public interface IContentItem
    {
        WebDocument Document { get; set; }
        IDictionary<WebVerb, IWebFeed> Feeds { get; }
    }
}