using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Set
    {
        public Kind Kind { get; set; }
        public Published Published { get; set; }
        public string[] Expand { get; set; }
        public string Locale { get; set; }

        public Set()
        {
            Expand = new string[] { };
        }
    }
}
