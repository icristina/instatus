using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Instatus.Data
{
    public interface IRepository<T> : IDisposable
    {
        IDbSet<T> Items { get; }
        int SaveChanges();
    }
}