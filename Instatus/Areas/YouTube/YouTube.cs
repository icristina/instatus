using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Areas.YouTube
{
    public static class YouTube
    {
        public static string Embed(string videoUri)
        {
            // from: http://youtu.be/Qjj7ZcDEIlw
            // to: http://www.youtube.com/embed/Qjj7ZcDEIlw?wmode=opaque
            var embedUri = string.Format("http://www.youtube.com/embed/{0}?wmode=opaque", videoUri.Substring(videoUri.LastIndexOf('/') + 1));
            var tag = new TagBuilder("iframe");

            tag.MergeAttribute("src", embedUri);
            tag.MergeAttribute("frameborder", "0");
            tag.MergeAttribute("allowfullscreen", null);
            
            return tag.ToString();            
        }
    }
}