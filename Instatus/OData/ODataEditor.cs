using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Instatus.OData
{
    public abstract class ODataEditor<T> : IEditor
    {
        private string entitySetName;
        
        public bool CanEdit(string uri)
        {
            return uri.Contains(entitySetName);
        }

        public bool CanDelete(string uri)
        {
            return uri.Contains(entitySetName);
        }

        public async Task<object> GetEditorAsync(string uri)
        {
            using (var httpClient = new HttpClient())
            {
                var stringResponse = await httpClient.GetStringAsync(uri);
                var entityModel = await JsonConvert.DeserializeObjectAsync<T>(stringResponse);

                return CreateViewModel(entityModel);
            }
        }

        protected abstract object CreateViewModel(T model);

        public async Task PatchAsync(string uri, object model)
        {
            using (var httpClient = new HttpClient())
            {
                var updates = GetDelta(model);

                // http://msdn.microsoft.com/en-us/library/ff478141.aspx
                XNamespace atom = "http://www.w3.org/2005/Atom";
                XNamespace ds = "http://schemas.microsoft.com/ado/2007/08/dataservices";
                XNamespace dsmd = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

                var content =
                  new XElement(dsmd + "properties",
                    updates.Select(u => new XElement(ds + u.Key, u.Value))
                  );

                var entry =
                  new XElement(atom + "entry",
                    new XElement(atom + "content",
                      new XAttribute("type", "application/xml"),
                      content)
                  );
                
                var stringContent = new StringContent(entry.ToString(), Encoding.UTF8, "application/atom+xml");
                
                var httpRequest = new HttpRequestMessage(new HttpMethod("PATCH"), uri) 
                { 
                    Content = stringContent 
                };

                var responseMessage = await httpClient.SendAsync(httpRequest);
            }
        }

        protected abstract IDictionary<string, object> GetDelta(dynamic viewModel);

        public async Task DeleteAsync(string uri)
        {
            using (var httpClient = new HttpClient())
            {
                await httpClient.DeleteAsync(uri);
            }
        }

        public ODataEditor(string entitySetName)
        {
            this.entitySetName = entitySetName;
        }
    }
}
