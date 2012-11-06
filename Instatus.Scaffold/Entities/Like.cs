using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Like
    {
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}