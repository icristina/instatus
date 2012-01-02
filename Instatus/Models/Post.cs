using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class Post : Page
    {
        public Post() { }

        public Post(string slug, string name, string body = null)
            : base(name)
        {
            Slug = slug;
            Document.Body = body;
        }
    }
}