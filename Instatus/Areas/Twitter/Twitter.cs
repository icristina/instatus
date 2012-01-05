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
        public static string Permalink(string user, string id)
        {
            return string.Format("http://twitter.com/{0}/status/{1}", user, id);
        }
        
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
                    Kind = "Tweet",
                    Source = "Twitter",
                    Description = includeEntities ? ReplaceEntitiesWithHtml(entry.text, entry.entities) : entry.text,
                    Timestamp = DateTime.Parse(entry.created_at),
                    Picture = entry.profile_image_url,
                    User = entry.user,
                    Uri = Twitter.Permalink(entry.from_user, entry.id_str)
                });
            }

            return feed;
        }

        // https://dev.twitter.com/docs/tweet-entities
        // https://github.com/danielcrenna/tweetsharp/blob/master/src/net40/TweetSharp.Next/Extensions/StringExtensions.cs
        private static string ReplaceEntitiesWithHtml(string text, dynamic entities) {
            var characters = text.ToCharArray().Select(c => c.ToString()).ToList();

            ReplaceLink(characters, entities.user_mentions, new Func<dynamic, object>(e => e.screen_name), "<a href=\"http://twitter.com/{0}\">{1}");
            ReplaceLink(characters, entities.urls, new Func<dynamic, object>(e => e.url), "<a href=\"{0}\">{1}");
            ReplaceLink(characters, entities.hashtags, new Func<dynamic, object>(e => e.text), "<a href=\"http://search.twitter.com/search?q={0}\">{1}");

            return string.Join("", characters);
        }

        private static void ReplaceLink(List<string> characters, dynamic list, Func<dynamic, object> resource, string formatString) {
            if (list != null)
            {
                foreach (var item in list)
                {
                    var startIndex = item.indices[0];
                    var endIndex = item.indices[1] - 1;

                    characters[startIndex] = string.Format(formatString, resource(item), characters[startIndex]);
                    characters[endIndex] = string.Format("{0}</a>", characters[endIndex]);
                }
            }
        }
    }
}