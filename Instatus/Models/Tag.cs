using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Dynamic;
using Instatus.Models;

namespace Instatus.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }

        public virtual Taxonomy Taxonomy { get; set; }
        public int? TaxonomyId { get; set; }

        public virtual Photo Picture { get; set; }
        public int? PictureId { get; set; }

        public virtual Source Source { get; set; }
        public int? SourceId { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Page> Pages { get; set; }
        
        [NotMapped]
        public dynamic Extensions { get; set; }

        public Tag(string name)
        {
            Name = name;
            Extensions = new ExpandoObject();
        }

        public Tag() { }

        public override string ToString()
        {
            return Name;
        }
    }
}

namespace Instatus
{
    public static class TagExtensions
    {
        public static IEnumerable<Tag> ByTaxonomy(this ICollection<Tag> tags, string taxonomyName)
        {
            return tags.Where(t => t.Taxonomy.Name == taxonomyName);
        }

        public static string Term(this ICollection<Tag> tags, string taxonomyName)
        {
            return tags.ByTaxonomy(taxonomyName).Select(t => t.Name).FirstOrDefault();
        }
    }
}