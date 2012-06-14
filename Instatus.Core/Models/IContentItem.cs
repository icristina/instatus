using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public interface IContentItem
    {
        string Name { get; set; }
        Document Document { get; set; }
    }
}