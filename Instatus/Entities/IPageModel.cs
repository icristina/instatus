using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Entities
{
    public interface IPageModel
    {
        IEnumerable<Page> GetPages(Query query);
        Page GetPage(string alias, Set set = null);
    }
}