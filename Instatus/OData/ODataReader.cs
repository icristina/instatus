using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.OData
{
    public class ODataReader<T> : IReader
    {
        private string entitySetName;
        
        public bool CanRead(string uri)
        {
            return uri.EndsWith(entitySetName);
        }

        public async Task<IList> GetListAsync(string uri)
        {
            using (var httpClient = new HttpClient())
            {
                var stringResponse = await httpClient.GetStringAsync(uri);
                var odataResponse = await JsonConvert.DeserializeObjectAsync<ODataResponse<T>>(stringResponse);

                return odataResponse.Value;
            }
            
            throw new NotImplementedException();
        }

        public ODataReader(string entitySetName)
        {
            this.entitySetName = entitySetName;
        }
    }
}
