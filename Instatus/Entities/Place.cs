using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Place
    {
        public int Id { get; set; }
        //public DbGeography Point { get; set; }
        public Address Address { get; set; }
        public string Title { get; set; }

        // associations
        public ICollection<Post> Posts { get; set; }

        public Place()
        {
            Address = new Address();

            // initialize collections
            Posts = new List<Post>();
        }
    }
}
