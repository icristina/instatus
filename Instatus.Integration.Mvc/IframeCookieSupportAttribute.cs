using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public class IframeCookieSupportAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            if (!filterContext.IsChildAction && !response.IsRequestBeingRedirected)
            {
                response.AddHeader("p3p", "CP=\"CAO PSA OUR\"");
            }
        }
    }
}
