using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Instatus.Models;
using Instatus.Web;

namespace Instatus.Data
{
    public interface IContentProvider
    {
        IEnumerable<Page> GetPages(WebExpression filter, WebStatus? status = WebStatus.Published);
        Page GetPage(string slug, string[] customExpansions = null);
    }
}