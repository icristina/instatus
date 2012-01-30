using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus
{
    public static class ActionExecutingContextExtensions
    {
        public static void ReturnFatalErrorResult(this ActionExecutingContext context, Exception exception, int statusCode = 500, string viewName = "FatalError")
        {
            // from mvc3 sources
            var controllerName = context.RouteData.ControllerName();
            var actionName = context.RouteData.ActionName();

            HandleErrorInfo model = new HandleErrorInfo(exception, controllerName, actionName);

            context.Result = new ViewResult
            {
                ViewName = viewName,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                TempData = context.Controller.TempData
            };

            context.HttpContext.Response.Clear();
            context.HttpContext.Response.StatusCode = statusCode;
            context.HttpContext.Response.TrySkipIisCustomErrors = true;     
        }
    }
}