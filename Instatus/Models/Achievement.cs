﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using System.ComponentModel.DataAnnotations;

namespace Instatus.Models
{
    public class Achievement : Page
    {
        public virtual ICollection<Award> Awards { get; set; } 
        
        public Achievement() : base() { }

        public Achievement(string name) : base(name) { } 
    }
}