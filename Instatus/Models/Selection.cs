using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Data;

namespace Instatus.Models
{
    public class Selection : IEntity
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual List List { get; set; }
        public int ListId { get; set; }

        public virtual Page Page { get; set; }
        public int PageId { get; set; }

        public Selection()
        {
            CreatedTime = DateTime.UtcNow;
        }
    }
}