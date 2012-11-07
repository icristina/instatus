using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Like
    {
        [Key, Column(Order = 0)]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}