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
using Instatus.Web;
using Newtonsoft.Json;

namespace Instatus
{
    public class FacebookClient : IDisposable
    {
        private HttpClient httpClient;        
        private string accessToken;
        private int limit;

        public Task<T> GetGraphApiAsync<T>(string path, string[] fields = null)
        {
            var requestUri = path
                .AppendQueryParameter("access_token", accessToken)
                .AppendQueryParameter("limit", limit)
                .AppendQueryParameter("fields", fields);

            var httpResponse = httpClient.GetAsync(requestUri).Result;

            httpResponse.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObjectAsync<T>(httpResponse.Content.ReadAsStringAsync().Result); // use ReadAsAsync<T> with RTM
        }

        public Task<User> Me()
        {
            return GetGraphApiAsync<User>("me");
        }

        public Task<Response<Connection>> Friends()
        {
            return GetGraphApiAsync<Response<Connection>>("me/friends");
        }

        public Task<Response<Post>> Posts(string connection = "me/posts", string[] fields = null)
        {
            return GetGraphApiAsync<Response<Post>>(connection, fields);
        }

        public Task<Response<Post>> NewsFeed()
        {
            return Posts("me/home");
        }

        public Task<Response<Post>> Timeline()
        {
            return Posts("me/feed");
        }

        public Task<Response<Friend>> AppFriends()
        {
            return GetGraphApiAsync<Response<Friend>>("fql?q=SELECT uid, name FROM user WHERE has_added_app=1 and uid IN (SELECT uid2 FROM friend WHERE uid1 = me())");
        }

        public void Dispose()
        {
            httpClient.TryDispose();
        }

        public FacebookClient(string accessToken, int limit = 25)
        {
            this.httpClient = new HttpClient()
            {
                //MaxResponseContentBufferSize = 1024 * 1024 * 10,
                BaseAddress = new Uri("https://graph.facebook.com"),                
            };
            this.accessToken = accessToken;
            this.limit = limit;
        }

        // https://developers.facebook.com/docs/reference/api/user/
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
}