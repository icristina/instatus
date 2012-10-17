using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface ICache
    {
        object Get(string key);
        void AddOrUpdate(string key, object value);
    }
}
