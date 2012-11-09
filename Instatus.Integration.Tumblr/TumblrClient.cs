using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Instatus.Integration.Json;
using Newtonsoft.Json;
using Instatus.Core.Utils;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Instatus.Core;
using Instatus.Integration.SystemNetHttp;

namespace Instatus.Integration.Tumblr
{
    // http://www.tumblr.com/docs/en/api/v2#posts
    public class TumblrClient : IDisposable
    {
        private HttpClient httpClient;
        private string accessToken;

        public Task<Payload<T>> GetApiAsync<T>(string path)
        {
            var uri = new PathBuilder(path)
                .Query("api_key", accessToken)
                .ToString();

            return httpClient.GetJsonResponse<Payload<T>>(uri);
        }

        public Task<Payload<PostList>> Posts()
        {
            return GetApiAsync<PostList>("posts");
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }

        public TumblrClient(string blogName, string accessToken)
        {
            this.accessToken = accessToken;
            this.httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://api.tumblr.com/v2/blog/" + blogName),
            };
        }

        public class Payload<T>
        {
            public Meta Meta { get; set; }
            public T Response { get; set; }
        }

        public class Meta 
        {
            public int Status { get; set; }
            [JsonProperty("msg")]
            public string Message { get; set; }
        }

        public class Info
        {
            public string Title { get; set; }
            public int Posts { get; set; }
            public string Name { get; set; }
            public int Updated { get; set; }
            public string Description { get; set; }
            public bool Ask { get; set; }
            public bool AskAnon { get; set; }
            public int Likes { get; set; }
        }

        public class Post
        {
            public string BlogName { get; set; }
            public int Id { get; set; }
            public string PostUrl { get; set; }
            public string Type { get; set; }
            public int Timestamp { get; set; }
            public string Date { get; set; }
            public string Format { get; set; }
            public string ReblogKey { get; set; }
            public string[] Tags { get; set; }
            public bool Bookmarklet { get; set; }
            public bool Mobile { get; set; }
            public string SourceUrl { get; set; }
            public string SourceTitle { get; set; }
            public bool Liked { get; set; }
            public string State { get; set; }

            // photo post
            public string Caption { get; set; }
            public Photo[] Photos { get; set; }

            // link post
            public string Title { get; set; }
            public string Url { get; set; }
            public string Description { get; set; }

            // video post
            public Player[] Player { get; set; }
        }

        public class Photo
        {
            public string Caption { get; set; }
            [JsonProperty("alt_sizes")]
            public Size[] Sizes { get; set; }
        }

        public class Size
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public string Url { get; set; }
        }

        public class Player
        {
            public int Width { get; set; }
            public string EmbedCode { get; set; }
        }

        public class PostList
        {
            public Info Blog { get; set; }
            public Post[] Posts { get; set; }
            public int TotalPosts { get; set; }
        }
    }
}
