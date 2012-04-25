using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Selection
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public Page Page { get; set; }
        public List List { get; set; }
        public int Quantity { get; set; }
        public int SortOrder { get; set; }

        public Selection()
        {
            CreatedTime = DateTime.UtcNow;
        }
    }
}
