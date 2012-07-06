using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http;

namespace Instatus.Integration.WebApi
{
    public static class DefaultWebApiConfig
    {
        public static void RegisterAll(HttpConfiguration configuration) 
        {
            configuration.Filters.Add(new LogExceptionFilter());
            configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
            
            var jsonFormatter = configuration.Formatters.JsonFormatter;           
            var jsonSerializationSettings = jsonFormatter.CreateDefaultSerializerSettings();
            
            jsonSerializationSettings.ContractResolver = new UnderscoreMappingResolver();
            jsonFormatter.SerializerSettings = jsonSerializationSettings;

            jsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("format", "json", "application/json"));

            var xmlFormatter = configuration.Formatters.XmlFormatter;

            xmlFormatter.MediaTypeMappings.Add(new QueryStringMapping("format", "xml", "application/xml"));
        }
    }
}
