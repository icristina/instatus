using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface IFilter
    {
        string Query { get; set; }
        string Sort { get; set; }
        string ContentType { get; set; }
        string[] Tags { get; set; }
        DateTime? StartTime { get; set; }
        DateTime? EndTime { get; set; }
    }
}
