using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Entities
{
    public class Segment
    {
        public DateTime? Date { get; set; }
        public string Text { get; set; }
        public double? Number { get; set; }
    }
}