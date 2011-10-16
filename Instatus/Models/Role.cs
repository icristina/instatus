using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Instatus.Web;

namespace Instatus.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public Role() { }

        public Role(string name)
        {
            Name = name;
        }

        public Role(WebRole role)
        {
            Name = role.ToString();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
