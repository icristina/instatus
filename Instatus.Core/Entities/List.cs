using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class List
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public User User { get; set; }
        public Application Application { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
#if NET45
        public ListType Type { get; set; }
#else
        public string Type { get; set; }
#endif

        public List()
        {
            CreatedTime = DateTime.UtcNow;
        }
    }

    public enum ListType
    {
        Favorite,
        Cart,
        Order,
        Reading,
        Wish,
        Navigation
    }
}
