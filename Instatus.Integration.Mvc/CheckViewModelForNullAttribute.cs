using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Instatus.Core;

namespace Instatus.Integration.Mvc
{
    public class CheckViewModelForNullAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Controller.ViewData.Model == null)
            {
                filterContext.Result = new HttpNotFoundResult();
            }
        }
    }
}
