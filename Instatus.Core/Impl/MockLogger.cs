using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core.Impl
{
    public class MockLogger : ILogger
    {
        public void Log(Exception exception, IDictionary<string, string> properties)
        {
            // do nothing
        }
    }
}
