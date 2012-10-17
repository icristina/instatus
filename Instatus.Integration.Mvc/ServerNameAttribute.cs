using Instatus.Core;
using Instatus.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public class ServerNameAttribute : ActionFilterAttribute
    {
        public IHosting Hosting { get; set; }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
 	         filterContext.RequestContext.HttpContext.Response.AddHeader("X-Cloud-ServerName", Hosting.ServerName);
        }
    }
}