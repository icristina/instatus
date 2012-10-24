using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Filters;
using Instatus.Core;

namespace Instatus.Integration.WebApi
{
    public class LogExceptionAttribute : ExceptionFilterAttribute
    {
        public IDictionary<string, string> GenerateProperties(HttpActionExecutedContext context)
        {
            return new Dictionary<string, string>()
            {
                { "Uri", context.Request.RequestUri.AbsoluteUri }
            };
        }
        
        public override void OnException(HttpActionExecutedContext context)
        {
            var logger = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILogger)) as ILogger;

            if (logger != null)
            {
                var properties = GenerateProperties(context);
                
                logger.Log(context.Exception, properties);
            }
        }
    }
}
