using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IAuditing
    {
        void Log(IPrincipal principal, string category, string uri, IDictionary<string, string> properties);
    }
}
