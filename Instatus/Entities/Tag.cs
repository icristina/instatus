using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
        public Taxonomy Taxonomy { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
