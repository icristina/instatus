using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Data;
using System.Text;
using Instatus.Models;
using System.IO;

namespace Instatus.Web
{
    public class HttpTraceAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (var db = WebApp.GetService<IApplicationContext>())
            {
                var message = new StringBuilder();
                var request = filterContext.HttpContext.Request;

                message.AppendSection("Request Body", request.InputStream.CopyToString());
                message.AppendSection("Server Variables", request.ServerVariables["ALL_RAW"]);

                db.Logs.Add(new Log()
                {
                    Verb = request.HttpMethod.ToCapitalized(),
                    Uri = request.RawUrl,
                    Message = message.ToString()
                });
                db.SaveChanges();
            }            
            
            base.OnActionExecuting(filterContext);
        }
    }
}