using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Entry
    {
        public string Uri { get; set; }
        public string Kind { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Caption { get; set; }
        public string User { get; set; }
        public string Source { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Rel { get; set; }
    }
}
