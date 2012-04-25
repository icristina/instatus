using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Taxonomy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Application Application { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
