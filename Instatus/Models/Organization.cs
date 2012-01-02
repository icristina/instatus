using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public class Organization : Place
    {
        public virtual ICollection<Profile> Profiles { get; set; }

        public Organization() { }

        public Organization(string name)
        {
            Name = name;
        }
    }
}