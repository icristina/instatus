using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class Region : Place
    {
        public Region() { }

        public Region(string name)
        {
            Name = name;
        }
    }
}