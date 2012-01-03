using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using System.Net;
using System.Text;

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
            var characters = text.ToCharArray().Select(c => c.ToString()).ToList();
            var length = text.Length - 1;

            if (entities.urls != null)
            {
                foreach (var link in entities.urls)
                {
                    var startIndex = link.indices[0];
                    var endIndex = link.indices[1] - 1;
                
                    characters[startIndex] = string.Format("<a href=\"{0}\">{1}", link.url, characters[startIndex]);
                    characters[endIndex] = string.Format("{0}</a>", characters[endIndex]);
                }
            }

            if (entities.hashtags != null)
            {
                foreach (var hashtag in entities.hashtags)
                {
                    var startIndex = hashtag.indices[0];
                    var endIndex = hashtag.indices[1] - 1;

                    characters[startIndex] = string.Format("<a href=\"http://search.twitter.com/search?q={0}\">{1}", hashtag.text, characters[startIndex]);
                    characters[endIndex] = string.Format("{0}</a>", characters[endIndex]);
                }
            }

            if (entities.user_mentions != null)
            {
                foreach (var mention in entities.user_mentions)
                {
                    var startIndex = mention.indices[0];
                    var endIndex = mention.indices[1] - 1;

                    characters[startIndex] = string.Format("<a href=\"http://twitter.com/{0}\">{1}", mention.screen_name, characters[startIndex]);
                    characters[endIndex] = string.Format("{0}</a>", characters[endIndex]);
                }
            }

            return string.Join("", characters);
        }
    }
}