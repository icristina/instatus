using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Instatus
{
    public class Fb : HttpClient
    {
        public class Client : HttpClient
        {
            private string accessToken;
            private int limit;

            public Task<T> GetGraphApiAsync<T>(string path)
            {
                var requestUri = path
                    .AppendQueryParameter("access_token", accessToken)
                    .AppendQueryParameter("limit", limit);

                var httpResponse = GetAsync(requestUri).Result;
                var formatters = new List<MediaTypeFormatter>() { 
                    new JsonNetFormatter(null) 
                };

                httpResponse.EnsureSuccessStatusCode();

                //return httpResponse.Content.ReadAsAsync<T>(formatters);
                return JsonConvert.DeserializeObjectAsync<T>(httpResponse.Content.ReadAsStringAsync().Result);
            }

            public Task<User> Me()
            {
                return GetGraphApiAsync<User>("me");
            }

            public Task<Response<Connection>> Friends()
            {
                return GetGraphApiAsync<Response<Connection>>("me/friends");
            }

            public Task<Response<Friend>> AppFriends()
            {
                return GetGraphApiAsync<Response<Friend>>("fql?q=SELECT uid, name FROM user WHERE has_added_app=1 and uid IN (SELECT uid2 FROM friend WHERE uid1 = me())");
            }

            public Client(string accessToken, int limit = 5000)
            {
                this.accessToken = accessToken;
                this.limit = limit;

                BaseAddress = new Uri("https://graph.facebook.com");
            }
        }

        public class User
        {
            public string id;
            public string first_name;
            public string middle_name;
            public string last_name;
            public string gender;
            public string locale;
            public string email;
        }

        public class Response<T>
        {
            public IList<T> data;
            public Paging paging;
        }

        public class Paging
        {
            public string next;
            public string previous;
        }

        public class Connection
        {
            public string id;
            public string name;
        }

        public class Friend
        {
            public string uid;
            public string name;
        }
    }

    // http://code.msdn.microsoft.com/Using-JSONNET-with-ASPNET-b2423706
    public class JsonNetFormatter : MediaTypeFormatter
    {
        private JsonSerializerSettings _jsonSerializerSettings;

        public JsonNetFormatter(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();

            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));

            Encoding = new UTF8Encoding(false, true);
        }

        protected override bool CanReadType(Type type)
        {
            if (type == typeof(IKeyValueModel))
            {
                return false;
            }

            return true;
        }

        protected override bool CanWriteType(Type type)
        {
            return true;
        }

        protected override Task<object> OnReadFromStreamAsync(Type type, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext)
        {
            JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);

            return Task.Factory.StartNew(() =>
            {
                using (StreamReader streamReader = new StreamReader(stream, Encoding))
                {
                    using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                    {
                        return serializer.Deserialize(jsonTextReader, type);
                    }
                }
            });
        }

        protected override Task OnWriteToStreamAsync(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext, TransportContext transportContext)
        {
            JsonSerializer serializer = JsonSerializer.Create(_jsonSerializerSettings);

            return Task.Factory.StartNew(() =>
            {
                using (StreamWriter streamWriter = new StreamWriter(stream, Encoding))
                {
                    using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
                    {
                        serializer.Serialize(jsonTextWriter, value);
                    }
                }
            });
        }
    }
}