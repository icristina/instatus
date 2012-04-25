using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Instatus.Web;
using Instatus.Models;

namespace Instatus.Entities
{
    public interface IContentModel
    {
        IEnumerable<Page> GetPages(Query query);
        Page GetPage(string slug, Set set = null);
    }
}