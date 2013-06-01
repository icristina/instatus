using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Models
{
    public class Place
    {
        public int Id { get; set; }

        // content
        public string Title { get; set; }

        // geospatial
        public Point Point { get; set; }
        public Address Address { get; set; }

        // social
        public string FacebookUri { get; set; }
        public string TwitterUri { get; set; }

        // links
        public string HomeUri { get; set; }

        // associations
        public virtual ICollection<Post> Posts { get; set; }

        public Place()
        {
            Point = new Point();
            Address = new Address();

            // initialize collections
            Posts = new List<Post>();
        }
    }
}
