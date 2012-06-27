using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface ILogger
    {
        void Log(Exception exception, IDictionary<string, string> properties);
    }
}
