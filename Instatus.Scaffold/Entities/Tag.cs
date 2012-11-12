using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Associations
        public virtual ICollection<Post> Posts { get; set; }
        public int TaxonomyId { get; set; }
        public virtual Taxonomy Taxonomy { get; set; }
    }
}