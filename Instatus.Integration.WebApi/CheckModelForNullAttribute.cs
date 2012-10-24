using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;

namespace Instatus.Integration.WebApi
{
    // http://www.strathweb.com/2012/10/clean-up-your-web-api-controllers-with-model-validation-and-null-check-filters/
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CheckModelForNullAttribute : ActionFilterAttribute
    {
        private readonly Func<Dictionary<string, object>, bool> condition;

        public CheckModelForNullAttribute()
            : this(arguments =>
                arguments.ContainsValue(null))
        { }

        public CheckModelForNullAttribute(Func<Dictionary<string, object>, bool> condition)
        {
            this.condition = condition;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (condition(actionContext.ActionArguments))
            {               
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cannot be null");
            }
        }
    }
}
