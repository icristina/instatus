using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Application
    {
        public int Id { get; set; }
        public int Locale { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Logo { get; set; }
        public Schedule Schedule { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Credential> Credentials { get; set; }
        public virtual ICollection<Domain> Domains { get; set; }
        public virtual ICollection<Taxonomy> Taxonomies { get; set; }
        public virtual ICollection<Phrase> Phrases { get; set; }

        public Application()
        {
            Schedule = new Schedule();
        }
    }
}
