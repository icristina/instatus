﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class Brand : Organization
    {
        public Brand() : base() { }

        public Brand(string name)
            : base(name)
        {

        }
    }
}