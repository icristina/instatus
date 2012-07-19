using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public class IframeCookieSupportAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            
            if (!filterContext.IsChildAction && !response.IsRequestBeingRedirected)
            {
                response.AddHeader("p3p", "CP=\"CAO PSA OUR\"");
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }
}
