using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Integration.Facebook
{
    public class FacebookClient : IDisposable
    {
        private HttpClient httpClient;
        private string accessToken;
        private const int DefaultLimit = 25;

        public async Task<T> GetGraphApiAsync<T>(string path, int limit = DefaultLimit, string[] fields = null)
        {
            var requestUri = path
                .AppendQueryParameter("access_token", accessToken)
                .AppendQueryParameter("limit", limit);

            if (fields != null)
                requestUri = requestUri.AppendQueryParameter("fields", string.Join(",", fields));

            var httpResponse = await httpClient.GetAsync(requestUri);

            httpResponse.EnsureSuccessStatusCode();

            var jsonMediaTypeFormatter = new JsonMediaTypeFormatter();

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

        public Task<Response<Connection>> Friends(int limit = DefaultLimit)
        {
            return GetGraphApiAsync<Response<Connection>>("me/friends", limit);
        }

        public Task<Response<Post>> Posts(string connection = "me/posts", int limit = DefaultLimit, string[] fields = null)
        {
            return GetGraphApiAsync<Response<Post>>(connection, limit, fields);
        }

        public Task<Response<Post>> Home(int limit = DefaultLimit, string[] fields = null)
        {
            return Posts("me/home", limit, fields);
        }

        public Task<Response<Post>> Feed(int limit = DefaultLimit, string[] fields = null)
        {
            return Posts("me/feed", limit, fields);
        }

        public Task<Response<Friend>> AppFriends(int limit = DefaultLimit)
        {
            return GetGraphApiAsync<Response<Friend>>("fql?q=SELECT uid, name FROM user WHERE has_added_app=1 and uid IN (SELECT uid2 FROM friend WHERE uid1 = me())", limit);
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }

        public FacebookClient(string accessToken)
        {
            this.accessToken = accessToken;
            this.httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://graph.facebook.com"),
            };
        }

        public class User
        {
            public string id;
            public string name;
            public string first_name;
            public string middle_name;
            public string last_name;
            public string gender;
            public string locale;
            public string username;
            public string third_party_id;
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
            public string category;
        }

        public class Friend
        {
            public string uid;
            public string name;
        }

        public class Post
        {
            public string id;
            public Connection from;
            public string message;
            public string picture;
            public string link;
            public string name;
            public string caption;
            public string description;
            public string icon;
            public Action[] actions;
            public Privacy privacy;
            public string type;
            public string object_id;
            public Connection application;
            public DateTime created_time;
            public DateTime updated_time;
            public Statistic comments;
            public string story;
            public IDictionary<string, StoryTag[]> story_tags;
            public Place place;
        }

        public class Action
        {
            public string name;
            public string link;
        }

        public class Privacy
        {
            public string description;
            public string value;
            public string allow;
            public string deny;
        }

        public class Statistic
        {
            public int count;
        }

        public class StoryTag
        {
            public long id;
            public string name;
            public int offset;
            public int length;
            public string type;
        }

        public class Place
        {
            public string id;
            public string name;
            public Location location;
        }

        public class Location
        {
            public string street;
            public string city;
            public string country;
            public string zip;
            public double latitude;
            public double longitude;
        }
    }

    internal static class PathExtensions
    {
        public static string AppendQueryParameter(this string uri, string name, object value)
        {
            if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(name) || value == null)
                return uri;

            var builder = new StringBuilder(uri)
                .Append(uri.Contains('?') ? '&' : '?')
                .Append(name)
                .Append('=')
                .Append(value);

            return builder.ToString();
        }
    }
}
