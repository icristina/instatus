using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Data
{
    public interface IContentQuery<T>
    {
        IQueryable<T> Query(IQueryable<T> queryable, Query query);
    }
}