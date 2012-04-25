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
        public static void LogChange(this IApplicationModel context, object resource, string action, string uri = null)
        {
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

        public static void LogChange(this IApplicationModel context, object resource, string propertyName, object originalValue, object newValue, string uri = null)
        {
            var action = string.Format("changed {0} from {1} to {2}", propertyName, originalValue, newValue);
            context.LogChange(resource, action, uri);
        }
    }
}