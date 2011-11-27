using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;

namespace Instatus.Models
{
    public class Media : Link
    {
        public int? Height { get; set; }
        public int? Width { get; set; }
        
        public Media() : base() { }
        public Media(WebContentType contentType, string uri) : base(contentType, uri) { }
    }
}