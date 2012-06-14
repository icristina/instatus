using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Request
    {
        public string Uri { get; set; }
        public string Method { get; set; }
        public object Parameters { get; set; }
    }
}
