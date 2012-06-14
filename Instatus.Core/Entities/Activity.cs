﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instatus.Models;

namespace Instatus.Entities
{
    public class Activity : IUserGeneratedContent
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
        public Page Page { get; set; }
        public int Rating { get; set; }
        public int Score { get; set; }
        public int Duration { get; set; }
        public Location Location { get; set; }
        public Application Application { get; set; }
        public string Verb { get; set; }
        public string Published { get; set; }

        public override string ToString()
        {
            return Description ?? base.ToString();
        }

        public Activity()
        {
            CreatedTime = DateTime.UtcNow;
            Location = new Location();
        }

        public Activity(Verb verb) : this()
        {
            Verb = verb.ToString();
        }
    }
}