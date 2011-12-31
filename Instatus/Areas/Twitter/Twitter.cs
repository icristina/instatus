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
        public static List<WebEntry> Search(params string[] terms)
        {
            var uriTemplate = "http://search.twitter.com/search.json?q={0}";
            var uri = string.Format(uriTemplate, HttpUtility.UrlEncode(string.Join(" OR ", terms)));
            var response = new WebClient().DownloadJson(uri);
            var feed = new List<WebEntry>();

            foreach (var entry in response.results)
            {
                feed.Add(new WebEntry()
                {
                    Description = entry.text,
                    Timestamp = DateTime.Parse(entry.created_at),
                    Picture = entry.profile_image_url,
                    User = entry.user 
                });
            }

            return feed;
        }
    }
}