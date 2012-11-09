using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Activity : ICreated, IPayload
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Kind { get; set; }
        public string Data { get; set; }
        public DateTime Created { get; set; }

        public Activity()
        {
            Created = DateTime.UtcNow;
        }
    }
}