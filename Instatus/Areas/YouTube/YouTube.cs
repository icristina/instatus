﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Web;

namespace Instatus.Areas.YouTube
{
    public static class YouTube
    {
        // http://stackoverflow.com/questions/6556772/parsing-youtube-url
        public static string ParseYouTubeId(string videoUri)
        {
            var uri = new Uri(videoUri);

            if (!uri.Query.IsEmpty()) {
                var queryString = HttpUtility.ParseQueryString(uri.Query);
                
                // http://www.youtube.com/watch?v=Lp7E973zozc&feature=relmfu
                if(queryString.AllKeys.Contains("v"))
                    return queryString["v"];
            }

            var embed = "/embed";

            // http://www.youtube.com/embed/sGE4HMvDe-Q
            if (uri.AbsolutePath.StartsWith(embed))
                return uri.AbsolutePath.Substring(embed.Length + 1);

            // http://youtu.be/sGE4HMvDe-Q
            // http://www.youtube.com/p/A0C3C1D163BE880A?hl=en_US&#038;fs=1 playlist
            return uri.AbsolutePath.Substring(1);
        }
        
        public static string Embed(string videoUri)
        {
            var youTubeId = ParseYouTubeId(videoUri);
            var embedUri = string.Format("https://www.youtube.com/embed/{0}?wmode=opaque&rel=0", youTubeId);  // rel=0 removes related videos end frame, https enables embed on https sites
            return HtmlBuilder.Embed(embedUri);       
        }
    }
}