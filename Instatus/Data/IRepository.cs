using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Instatus.Data
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IDbSet<T> Items { get; }
        int SaveChanges();
    }
}