using Instatus.Core;
using Instatus.Core.Models;
using Instatus.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    // http://my.safaribooksonline.com/book/-/9781449321932
    public class MultipleResponseFormatsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var viewResult = filterContext.Result as ViewResult;

            if (viewResult == null)
                return;

            if (request.IsAjaxRequest() || filterContext.IsChildAction)
            {
                filterContext.Result = new PartialViewResult
                {
                    TempData = viewResult.TempData,
                    ViewData = viewResult.ViewData,
                    ViewName = viewResult.ViewName,
                };
            }
            else if (request.AcceptTypes.Any(a => a.Contains("application/json")))
            {
                filterContext.Result = new JsonResult
                {
                    Data = viewResult.Model
                };
            }
        }
    }
}