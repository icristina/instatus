using Elmah;
using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using elmah = Elmah;

namespace Instatus.Integration.Elmah
{
    public class ElmahLogger : ILogger
    {
        public void Log(Exception exception, IDictionary<string, string> properties)
        {
            if (HttpContext.Current != null)
            {
                ErrorSignal.FromCurrentContext().Raise(exception);
            }
            else
            {
                elmah.ErrorLog.GetDefault(null).Log(new elmah.Error(exception));
            }
        }
    }
}
