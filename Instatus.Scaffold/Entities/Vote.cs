using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Vote : ICreated
    {
        public int Id { get; set; }
        public int Choice { get; set; }
        public int PollId { get; set; }
        public virtual Poll Poll { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime Created { get; set; }
    }
}