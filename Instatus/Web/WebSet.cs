using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Instatus.Web
{
    public class WebSet
    {
        public string Path { get; set; } // application or folder
        public string Locale { get; set; }
        public string[] Expand { get; set; }
        public WebStatus Status { get; set; }
        public WebKind Kind { get; set; }
    } 
}