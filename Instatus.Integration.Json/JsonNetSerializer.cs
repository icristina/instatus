using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Core;
using Newtonsoft.Json;

namespace Instatus.Integration.Json
{
    public class JsonNetSerializer : IJsonSerializer
    {
        private JsonSerializerSettings Settings
        {
            get
            {
                return new JsonSerializerSettings() 
                {
                    ContractResolver = new UnderscoreMappingResolver()    
                };
            }
        }
        
        public T Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        public string Stringify(object graph)
        {
            return JsonConvert.SerializeObject(graph, Settings);
        }
    }
}
