using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Instatus.Web;

namespace Instatus.Models
{
    [KnownType(typeof(Photo))]
    [KnownType(typeof(Video))]
    public class Link
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public string Uri { get; set; }
        public string AlternativeUri { get; set; }
        public string ContentType { get; set; }
        public string Credit { get; set; }

        public string Rel { get; set; }
        public int Priority { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        public Link() { }

        public Link(WebContentType contentType, string uri)
        {
            ContentType = contentType.ToMimeType();
            Uri = uri;
        }

        public override string ToString()
        {
            return Uri;
        }
    }
}