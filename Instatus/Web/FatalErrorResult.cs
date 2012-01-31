using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Web
{
    public class FatalErrorResult : ViewResult
    {
        public FatalErrorResult(ControllerContext context, Exception exception, int statusCode = 500, string viewName = "FatalError")
        {
            // modified from HandlerErrorAttribute mvc3 sources
            var controllerName = context.RouteData.ControllerName();
            var actionName = context.RouteData.ActionName();
            var model = new HandleErrorInfo(exception, controllerName, actionName);

            ViewName = viewName;
            ViewData = new ViewDataDictionary<HandleErrorInfo>(model);
            TempData = context.Controller.TempData;

            context.HttpContext.Response.Clear();
            context.HttpContext.Response.StatusCode = statusCode;
            context.HttpContext.Response.TrySkipIisCustomErrors = true;  
        }
    }
}