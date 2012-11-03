using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Score : ICreated
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int Points { get; set; }
        public int Time { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}