﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebInsight
    {
        public int Total { get; set; }
        public bool Completed { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string User { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
    }
}