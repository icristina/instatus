﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public string Text { get; set; }
        public State State { get; set; }

        // associations
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public int? CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
    }
}
