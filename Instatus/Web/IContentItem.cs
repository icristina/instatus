using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using Instatus.Models;

namespace Instatus.Web
{
    public interface IContentItem
    {
        string Name { get; set; }
        Document Document { get; set; }
    }
}