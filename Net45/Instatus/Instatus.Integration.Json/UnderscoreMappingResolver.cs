using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace Instatus.Integration.Json
{
    // http://stackoverflow.com/questions/3922874/c-sharp-json-net-convention-that-follows-ruby-property-naming-conventions
    public class UnderscoreMappingResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return char.IsUpper(propertyName[0]) ? // if first character is lowercase, assume already customized with [JsonProperty] attribute
                Regex.Replace(propertyName, @"([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", "$1$3_$2$4")
                    .ToLower() : propertyName;
        }
    }
}
