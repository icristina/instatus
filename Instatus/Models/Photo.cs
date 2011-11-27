using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;

namespace Instatus.Models
{
    public class Photo : Media
    {
        public Photo() : base() { }
        public Photo(WebContentType contentType, string uri) : base(contentType, uri) { }
    }
}