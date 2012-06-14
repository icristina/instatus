using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Domain
    {
        public int Id { get; set; }
        public string Hostname { get; set; }
        public bool Canonical { get; set; }
        public Application Application { get; set; }
        public int ApplicationId { get; set; }
        public string Environment { get; set; }

        public override string ToString()
        {
            return Hostname;
        }
    }
}
