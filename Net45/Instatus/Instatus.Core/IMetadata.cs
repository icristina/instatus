using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IMetadata
    {
        string ContentType { get; set; }
        IDictionary<string, string> Headers { get; set; }
    }
}
