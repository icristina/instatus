using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Data;

namespace Instatus.Models
{
    public class Entry : ITimestamp
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
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
