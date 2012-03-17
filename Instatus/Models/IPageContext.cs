using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Instatus.Models;
using Instatus.Web;

namespace Instatus.Models
{
    public interface IPageContext
    {
        IEnumerable<Page> GetPages(WebQuery query);
        Page GetPage(string slug, WebSet set = null);
    }
}