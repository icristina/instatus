using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;
using Instatus.Web;

namespace Instatus.Models
{
    public class List : IEntity, INamed
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
        public double Total { get; set; }
        public int Priority { get; set; }
        public string Permissions { get; set; }
        public string Status { get; set; }

        public virtual User User { get; set; }
        public int? UserId { get; set; }

        public virtual ICollection<Selection> Selections { get; set; }

        public List()
        {
            CreatedTime = DateTime.UtcNow;
        }

        public List(WebListType type, User user) : this()
        {
            Type = type.ToString();
            User = user;
        }
    }
}