using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Instatus.Core;

namespace Instatus.Integration.Mvc
{
    public class LogExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public ILogger Logger { get; set; }
        
        public IDictionary<string, string> GenerateProperties(ExceptionContext context)
        {
            return new Dictionary<string, string>()
            {
                { "Uri", context.HttpContext.Request.Url.AbsoluteUri }
            };
        }        
        
        public void OnException(ExceptionContext context)
        {
            if (Logger != null)
            {
                Logger.Log(context.Exception, GenerateProperties(context));
            }
        }
    }
}
