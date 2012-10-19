using Elmah;
using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Integration.Elmah
{
    public class ElmahLogger : ILogger
    {
        public void Log(Exception exception, IDictionary<string, string> properties)
        {
            ErrorSignal.FromCurrentContext().Raise(exception);
        }
    }
}
