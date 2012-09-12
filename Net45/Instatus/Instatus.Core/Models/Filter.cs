using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Models
{
    public class Filter : IFilter
    {
        public string Query { get; set; }
        public string Sort { get; set; }
        public string ContentType { get; set; }
        public string[] Tags { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
