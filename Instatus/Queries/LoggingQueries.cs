using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using System.Text;
using System.ServiceModel.Web;
using Instatus.Models;
using Instatus.Web;
using System.Data.Objects;

namespace Instatus
{
    public static class LoggingQueries
    {
        public static void LogError(this BaseDataContext context, Exception error)
        {
            if (!WebBootstrap.LoggingEnabled)
                return;

            var message = new StringBuilder();
            var uri = string.Empty;

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
                uri = HttpContext.Current.Request.RawUrl;

                if (WebOperationContext.Current == null)
                {
                    message.AppendSection("Server Variables", HttpContext.Current.Request.ServerVariables["ALL_RAW"]);
                }
            }

            context.Logs.Add(new Log()
            {
                Verb = WebVerb.Error.ToString(),
                Uri = uri,
                Message = message.ToString()
            });
        }

        public static void LogChange(this BaseDataContext context, object resource, string action, string uri = null)
        {
            if (!WebBootstrap.LoggingEnabled)
                return;

            var now = DateTime.UtcNow;
            var user = context.GetCurrentUser();
            var description = resource is string ? resource : string.Format("{0} {1}", ObjectContext.GetObjectType(resource.GetType()).Name, resource.GetKey());
            var message = string.Format("{0} {1} for {2} at {3}",
                user.FullName,
                action,
                description,
                now);

            context.Logs.Add(new Log()
            {
                Verb = WebVerb.Change.ToString(),
                Uri = uri,
                User = user,
                Message = message,
                CreatedTime = now
            });
        }

        public static void LogChange(this BaseDataContext context, object resource, string propertyName, object originalValue, object newValue, string uri = null)
        {
            var action = string.Format("changed {0} from {1} to {2}", propertyName, originalValue, newValue);
            context.LogChange(resource, action, uri);
        }
    }
}