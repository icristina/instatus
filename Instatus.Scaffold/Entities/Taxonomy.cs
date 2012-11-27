using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Taxonomy
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Associations
        public virtual ICollection<Tag> Tags { get; set; }

        public Taxonomy()
        {
            // initialize collections
            Tags = new List<Tag>();
        }
    }
}