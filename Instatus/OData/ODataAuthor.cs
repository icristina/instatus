using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.OData
{
    public abstract class ODataAuthor<T> : IAuthor
    {
        private string entitySetName;
        
        public bool CanCreate(string uri)
        {
            return uri.Contains(entitySetName);
        }

        public abstract object CreateInstance();

        public async Task<string> PostAsync(object model)
        {
            using (var httpClient = new HttpClient())
            {
                var entityModel = CreateModel(model);
                var jsonString = await JsonConvert.SerializeObjectAsync(entityModel);
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                await httpClient.PostAsync(string.Empty, stringContent);

                return string.Empty;
            }
        }

        protected abstract T CreateModel(object viewModel);

        public ODataAuthor(string entitySetName)
        {
            this.entitySetName = entitySetName;
        }
    }
}
