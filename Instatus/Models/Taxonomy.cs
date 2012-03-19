using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;

namespace Instatus.Models
{
    public class Taxonomy : IEntity, INamed
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Application> Applications { get; set; }

        public Taxonomy() { 
            Tags = new List<Tag>();
        }

        public Taxonomy(string name)
            : this()
        {
            Name = name;
        }

        public Taxonomy(string name, string[] tags) : this()
        {
            Name = name;
            tags.ToList().ForEach(t => Tags.Add(new Tag(t)));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}