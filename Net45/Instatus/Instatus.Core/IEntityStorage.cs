using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IEntityStorage
    {
        IEntitySet<T> Set<T>();
        void SaveChanges();
    }
}
