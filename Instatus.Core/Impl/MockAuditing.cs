using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core.Impl
{
    public class MockAuditing : IAuditing
    {
        public void Log(IPrincipal principal, string category, string uri, IDictionary<string, string> properties)
        {
            // do nothing
        }
    }
}
