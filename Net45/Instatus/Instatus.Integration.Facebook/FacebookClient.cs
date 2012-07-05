﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Instatus.Core.Utils;

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
