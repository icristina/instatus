﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Instatus.Web
{
    public class WebSet
    {
        public string Path { get; set; } // application or folder
        public string[] Expand { get; set; }
        public string Locale { get; set; }
        public WebStatus Status { get; set; }
        public WebKind Kind { get; set; }

        public WebSet()
        {
            Expand = new string[] {};
        }
    } 
}