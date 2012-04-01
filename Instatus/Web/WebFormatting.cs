using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebFormatting
    {
        public string Label { get; set; }
        public int MaxLength { get; set; }

        public WebFormatting()
        {
            MaxLength = int.MaxValue;
        }
    }
}