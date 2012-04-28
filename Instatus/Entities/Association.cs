using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Association
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Page Parent { get; set; }
        public int ParentId { get; set; }
        public virtual Page Child { get; set; }
        public int ChildId { get; set; }
        public int? SortOrder { get; set; }
    }
}
