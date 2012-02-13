using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Data;
using Instatus.Models;
using System.Threading.Tasks;
using System.Text;

namespace Instatus.Web
{
    public class EmptyResultOnExceptionAttribute : FilterAttribute, IExceptionFilter
    {                
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = new EmptyResult();
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Cache.IgnoreThisRequest();
        }
    }
}