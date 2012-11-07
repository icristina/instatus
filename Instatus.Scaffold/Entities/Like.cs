﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Like
    {
        [Key]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        [Key]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}