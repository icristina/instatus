using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using System.Net;

namespace Instatus.Areas.Twitter
{
    public static class Twitter
    {
        public const string TagUriTemplate = "http://search.twitter.com/search?q={0}";
        public const string MentionUriTemplate = "http://twitter.com/{0}";
        
        public static List<WebEntry> Search(string[] terms, bool includeEntities = true)
        {
            var uriTemplate = "http://search.twitter.com/search.json?q={0}&include_entities={1}";
            var uri = string.Format(uriTemplate, HttpUtility.UrlEncode(string.Join(" OR ", terms)), includeEntities);
            var response = new WebClient().DownloadJson(uri);
            var feed = new List<WebEntry>();

            foreach (var entry in response.results)
            {
                feed.Add(new WebEntry()
                {
                    Description = includeEntities ? ReplaceEntitiesWithHtml(entry.text, entry.entities) : entry.text,
                    Timestamp = DateTime.Parse(entry.created_at),
                    Picture = entry.profile_image_url,
                    User = entry.user 
                });
            }

            return feed;
        }

        // https://dev.twitter.com/docs/tweet-entities
        // https://github.com/danielcrenna/tweetsharp/blob/master/src/net40/TweetSharp.Next/Extensions/StringExtensions.cs
        private static string ReplaceEntitiesWithHtml(string text, dynamic entities) {
            return text;
        }
    }
}