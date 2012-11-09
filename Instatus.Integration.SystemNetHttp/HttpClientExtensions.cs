using Instatus.Core;
using Instatus.Integration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Integration.SystemNetHttp
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetJsonResponse<T>(this HttpClient httpClient, string uri) 
        {
            var httpResponse = await httpClient.GetAsync(uri);

            httpResponse.EnsureSuccessStatusCode();

            var jsonMediaTypeFormatter = new JsonMediaTypeFormatter();
            var jsonSerializationSettings = jsonMediaTypeFormatter.CreateDefaultSerializerSettings();

            jsonSerializationSettings.ContractResolver = new UnderscoreMappingResolver();

            jsonMediaTypeFormatter.SerializerSettings = jsonSerializationSettings;
            jsonMediaTypeFormatter.SupportedMediaTypes.Clear();
            jsonMediaTypeFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(WellKnown.ContentType.Js));

            return await httpResponse.Content.ReadAsAsync<T>(new MediaTypeFormatter[] 
            { 
                jsonMediaTypeFormatter 
            });
        }

        public static async Task<string> GetStringResponse(this HttpClient httpClient, string uri)
        {
            var httpResponse = await httpClient.GetAsync(uri);

            httpResponse.EnsureSuccessStatusCode();

            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
}
