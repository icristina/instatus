using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace Instatus.Integration.Elmah
{
    public class ElmahLoggerFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            new ElmahLogger().Log(actionExecutedContext.Exception, null);
        }
    }
}
