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
        public Page Parent { get; set; }
        public Page Child { get; set; }
        public int SortOrder { get; set; }
    }
}
