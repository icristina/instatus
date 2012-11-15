using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core.Impl
{
    public class MockProfiler : IProfiler
    {
        public IDisposable Step(string label)
        {
            return null; // using(null) does not throw error
        }
    }
}
