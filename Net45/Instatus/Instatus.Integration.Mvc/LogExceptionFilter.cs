using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Instatus.Core;

namespace Instatus.Integration.Mvc
{
    public class LogErrorFilter : FilterAttribute, IExceptionFilter
    {
        public IDictionary<string, string> GenerateProperties(ExceptionContext context)
        {
            return new Dictionary<string, string>()
            {
                { "Uri", context.HttpContext.Request.Url.AbsoluteUri }
            };
        }        
        
        public void OnException(ExceptionContext context)
        {
            var logger = DependencyResolver.Current.GetService<ILogger>();

            if (logger != null)
            {
                var properties = GenerateProperties(context);

                logger.Log(context.Exception, properties);
            }
        }
    }
}
