using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Data;
using Instatus.Models;
using System.Threading.Tasks;
using System.Text;

namespace Instatus.Web
{
    public class LogErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            using (var db = BaseDataContext.Instance())
            {
                if (db == null)
                    return;
                
                var message = new StringBuilder();

                message.AppendSection("Message", filterContext.Exception.Message);
                message.AppendSection("Stack Trace", filterContext.Exception.StackTrace);
                message.AppendSection("Server Variables", filterContext.HttpContext.Request.ServerVariables["ALL_RAW"]);

                db.Logs.Add(new Log()
                {
                    Verb = "Error",
                    Uri = filterContext.HttpContext.Request.RawUrl,
                    Message = message.ToString()
                });
                db.SaveChanges();
            }
        }
    }

    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendSection(this StringBuilder sb, string title, string body)
        {
            sb.AppendLine();
            sb.AppendFormat("<section title=\"{0}\">", title);
            sb.AppendLine(body);
            sb.AppendLine("</section>");
            return sb;
        }
    }
}