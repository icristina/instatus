using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Web
{
    public interface IContentItem : IExtensionPoint
    {
        string Name { get; set; }
        WebDocument Document { get; set; }
        IDictionary<WebVerb, IWebFeed> Feeds { get; }
    }
}