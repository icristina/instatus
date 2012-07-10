using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class BaseMetadata : IMetadata
    {
        public string ContentType { get; set; }
        public IDictionary<string, string> Headers { get; private set; }

        public BaseMetadata()
        {
            Headers = new Dictionary<string, string>();
        }
    }
}
