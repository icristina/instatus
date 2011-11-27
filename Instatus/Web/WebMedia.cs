using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebMedia : WebLink
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int Duration { get; set; }
    }
}