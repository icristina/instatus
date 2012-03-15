using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;

namespace Instatus.Models
{
    public class Subscription : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Application Application { get; set; }
        public int? ApplicationId { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}