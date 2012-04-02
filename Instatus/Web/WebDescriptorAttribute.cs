using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebDescriptorAttribute : Attribute
    {
        public string Scope { get; set; }
    }
}