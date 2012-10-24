using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Instatus.Integration.WebApi
{
    // http://www.strathweb.com/2012/10/clean-up-your-web-api-controllers-with-model-validation-and-null-check-filters/
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CheckModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}
