using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class Article : Page
    {
        public Article() : base() { }

        public Article(string slug, string name, string body = null)
            : base(name)
        {
            Slug = slug;
            Document.Body = body;
        }
    }
}