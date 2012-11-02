using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core.Impl
{
    public class MockCache : ICache
    {
        public object Get(string key)
        {
            return null;
        }

        public void AddOrUpdate(string key, object value)
        {
            // do nothing
        }
    }
}
