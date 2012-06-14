using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Redirect
    {
        public int Id { get; set; }
        public int HttpStatusCode { get; set; }
        public string Source { get; set; }
        public string Location { get; set; }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", Source, Location);
        }
    }
}
