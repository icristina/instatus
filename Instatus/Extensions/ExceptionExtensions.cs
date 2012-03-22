using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Instatus
{
    public static class ExceptionExtensions
    {
        public static string ToHtml(this Exception error)
        {
            var message = new StringBuilder();

            message.AppendSection("Uri", error.GetUri());
            message.AppendSection("Message", error.Message);
            message.AppendSection("Stack Trace", error.StackTrace);

            var innerException = error.InnerException;

            if (innerException != null)
            {
                message.AppendSection("Inner Exception Message", innerException.Message);
                message.AppendSection("Inner Exception Stack Trace", innerException.StackTrace);
            }

            if (HttpContext.Current.Request != null)
            {
                message.AppendSection("Server Variables", HttpContext.Current.Request.ServerVariables["ALL_RAW"]);
            }

            return message.ToString();
        }

        public static string GetUri(this Exception error)
        {
            return HttpContext.Current.Request != null ? HttpContext.Current.Request.RawUrl : string.Empty;
        }
    }
}