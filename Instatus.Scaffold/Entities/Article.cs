using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public string Locale { get; set; }
    }
}