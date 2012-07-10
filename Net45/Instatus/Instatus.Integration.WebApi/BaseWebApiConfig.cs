using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http;
using System.Web.Routing;

namespace Instatus.Integration.WebApi
{
    public static class BaseWebApiConfig
    {
        public static void RegisterDefaultRoute(RouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void RegisterErrorDetailPolicy(HttpConfiguration configuration)
        {
            configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
        }

        public static void RegisterExceptionFilters(HttpConfiguration configuration)
        {
            configuration.Filters.Add(new LogExceptionFilter());
        }

        public static void RegisterMethodOverrides(HttpConfiguration configuration)
        {
            var overrideMethods = new HttpMethod[] { HttpMethod.Put, HttpMethod.Get }; // allow Flash to send custom headers in Flash via POST

            configuration.MessageHandlers.Add(new HttpMethodOverrideHandler(overrideMethods));
        }

        public static void RegisterJsonFormatter(HttpConfiguration configuration) 
        {
            var jsonFormatter = configuration.Formatters.JsonFormatter;           
            var jsonSerializationSettings = jsonFormatter.CreateDefaultSerializerSettings();
            
            jsonSerializationSettings.ContractResolver = new UnderscoreMappingResolver();
            jsonFormatter.SerializerSettings = jsonSerializationSettings;
        }

        public static void RegisterQueryStringMediaTypeMappings(HttpConfiguration configuration)
        {
            var jsonFormatter = configuration.Formatters.JsonFormatter;
            
            jsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("format", "json", "application/json"));

            var xmlFormatter = configuration.Formatters.XmlFormatter;

            xmlFormatter.MediaTypeMappings.Add(new QueryStringMapping("format", "xml", "application/xml"));
        }
    }
}
