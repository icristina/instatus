using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IEntitySet<T> : IQueryable<T>
    {
        T Find(object key);
        void Delete(object key);
        T Create();
    }
}
