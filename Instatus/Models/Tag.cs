using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // associations
        public virtual ICollection<Post> Posts { get; set; }

        public Tag()
        {
            // initialize collections
            Posts = new List<Post>();
        }
    }
}
