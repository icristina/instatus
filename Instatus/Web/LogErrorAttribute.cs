using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Data;
using Instatus.Models;
using System.Threading.Tasks;
using System.Text;
using Instatus.Services;
using System.ComponentModel.Composition;

namespace Instatus.Web
{
    public class LogErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var loggingService = DependencyResolver.Current.GetService<ILoggingService>();
            
            if(loggingService != null)
                loggingService.Log(filterContext.Exception);
        }
    }
}