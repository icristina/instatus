using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;

namespace Instatus.Models
{
    public class Video : Media
    {
        public int? Duration { get; set; }
        
        public Video() { }
        public Video(WebContentType contentType, string uri) : base(contentType, uri) { }
    }
}