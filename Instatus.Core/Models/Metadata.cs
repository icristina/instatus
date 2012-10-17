﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Models
{
    public class Metadata
    {
        public string ContentType { get; set; }
        public IDictionary<string, string> Headers { get; private set; }

        public Metadata()
        {
            Headers = new Dictionary<string, string>();
        }
    }
}