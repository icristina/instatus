using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Instatus.Web;
using Instatus.Data;

namespace Instatus.Models
{
    [KnownType(typeof(Photo))]
    [KnownType(typeof(Video))]
    [KnownType(typeof(Media))]
    public class Link : IEntity, INamed, INavigableContent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Uri { get; set; }
        public string Location { get; set; }
        public string Picture { get; set; }
        public string ContentType { get; set; }
        public string Credit { get; set; }
        public int HttpStatusCode { get; set; }
        public string Rel { get; set; }
        public int Priority { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        public Link() { }

        public Link(WebContentType contentType, string uri)
        {
            ContentType = contentType.ToMimeType();
            Uri = uri;
            HttpStatusCode = 200;
        }

        public override string ToString()
        {
            return Uri;
        }
    }
}