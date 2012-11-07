using Instatus.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Subscription : ICreated, IPayload
    {
        [Key, Column(Order = 0)]
        public int ListId { get; set; }
        public virtual List List { get; set; }
        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime Created { get; set; }
        public string Data { get; set; }

        public Subscription()
        {
            Created = DateTime.UtcNow;
        }
    }
}