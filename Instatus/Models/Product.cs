﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class Product : Page
    {
        public virtual ICollection<Price> Prices { get; set; }

        public Product() : base() { }

        public Product(string name)
            : base()
        {
            Name = name;
        }
    }
}