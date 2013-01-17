using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Models
{
    public class Metadata
    {
        public bool PublicRead { get; set; }
        public string ContentType { get; set; }
        public IDictionary<string, string> Headers { get; private set; }

        public Metadata()
        {
            PublicRead = true;
            Headers = new Dictionary<string, string>();
        }
    }
}
