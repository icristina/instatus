using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core.Utils;
using Instatus.Integration.Json;

namespace Instatus.Integration.Facebook
{
    public class FacebookClient : IDisposable
    {
        private HttpClient httpClient;
        private const int defaultLimit = 25;

        public string AccessToken { get; set; }

        public async Task<string> GetAppAccessToken(string applicationId, string privateKey)
        {
            var requestUri = new PathBuilder("https://graph.facebook.com/oauth/access_token")
                .Query("grant_type", "client_credentials")
                .Query("client_id", applicationId)
                .Query("client_secret", privateKey)
                .ToString();

            var httpResponse = await httpClient.GetAsync(requestUri);

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            AccessToken = stringResponse.Substring(13);

            return AccessToken;
        }

        public async Task<T> GetGraphApiAsync<T>(string path, int limit = defaultLimit, string[] fields = null)
        {
            var uri = new PathBuilder(path)
                .Query("access_token", AccessToken)
                .Query("limit", limit)
                .Query("fields", fields)
                .ToString();

            var httpResponse = await httpClient.GetAsync(uri);

            httpResponse.EnsureSuccessStatusCode();

            var jsonMediaTypeFormatter = new JsonMediaTypeFormatter();
            var jsonSerializationSettings = jsonMediaTypeFormatter.CreateDefaultSerializerSettings();

            jsonSerializationSettings.ContractResolver = new UnderscoreMappingResolver();
            
            jsonMediaTypeFormatter.SerializerSettings = jsonSerializationSettings;
            jsonMediaTypeFormatter.SupportedMediaTypes.Clear();
            jsonMediaTypeFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));

            return await httpResponse.Content.ReadAsAsync<T>(new MediaTypeFormatter[] 
            { 
                jsonMediaTypeFormatter 
            });
        }

        public Task<User> Me()
        {
            return GetGraphApiAsync<User>("me");
        }

        public Task<Response<Connection>> Friends(int limit = defaultLimit)
        {
            return GetGraphApiAsync<Response<Connection>>("me/friends", limit);
        }

        public Task<Response<Post>> Posts(string connection = "me/posts", int limit = defaultLimit, string[] fields = null)
        {
            return GetGraphApiAsync<Response<Post>>(connection, limit, fields);
        }

        public Task<Response<Post>> Home(int limit = defaultLimit, string[] fields = null)
        {
            return Posts("me/home", limit, fields);
        }

        public Task<Response<Post>> Feed(int limit = defaultLimit, string[] fields = null)
        {
            return Posts("me/feed", limit, fields);
        }

        public Task<Response<Friend>> AppFriends(int limit = defaultLimit)
        {
            return GetGraphApiAsync<Response<Friend>>("fql?q=SELECT uid, name FROM user WHERE has_added_app=1 and uid IN (SELECT uid2 FROM friend WHERE uid1 = me())", limit);
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }

        public FacebookClient(string accessToken)
        {
            this.AccessToken = accessToken;
            this.httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://graph.facebook.com"),
            };
        }

        public class User
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            public string Locale { get; set; }
            public string Username { get; set; }
            public string ThirdPartyId { get; set; }
            public string Email { get; set; }
        }

        public class Response<T>
        {
            public IList<T> Data { get; set; }
            public Paging Paging { get; set; }
        }

        public class Paging
        {
            public string Next { get; set; }
            public string Previous { get; set; }
        }

        public class Connection
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
        }

        public class Friend
        {
            public string Uid { get; set; }
            public string Name { get; set; }
        }

        public class Post
        {
            public string Id { get; set; }
            public Connection From { get; set; }
            public string Message { get; set; }
            public string Picture { get; set; }
            public string Link { get; set; }
            public string Name { get; set; }
            public string Caption { get; set; }
            public string Description { get; set; }
            public string Icon { get; set; }
            public Action[] Actions { get; set; }
            public Privacy Privacy { get; set; }
            public string Type { get; set; }
            public string ObjectId { get; set; }
            public Connection Application { get; set; }
            public DateTime CreatedTime { get; set; }
            public DateTime UpdatedTime { get; set; }
            public Statistic Comments { get; set; }
            public string Story { get; set; }
            public IDictionary<string, StoryTag[]> StoryTags { get; set; }
            public Place Place { get; set; }
        }

        public class Action
        {
            public string Name { get; set; }
            public string Link { get; set; }
        }

        public class Privacy
        {
            public string Description { get; set; }
            public string Value { get; set; }
            public string Allow { get; set; }
            public string Deny { get; set; }
        }

        public class Statistic
        {
            public int Count { get; set; }
        }

        public class StoryTag
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public int Offset { get; set; }
            public int Length { get; set; }
            public string Type { get; set; }
        }

        public class Place
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public Location Location { get; set; }
        }

        public class Location
        {
            public string Street { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Zip { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
