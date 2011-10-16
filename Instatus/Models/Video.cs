using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;

namespace Instatus.Models
{
    public class Video : Link
    {
        public Video() : base() { }
        public Video(WebContentType contentType, string uri) : base(contentType, uri) { }
    }
}