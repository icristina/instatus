using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Models
{
    public class Comment
    {
        public int Id { get; set; }

        // content
        public string Title { get; set; }
        public string Text { get; set; }

        // publishing
        public State State { get; set; }
        public DateTime Created { get; set; }

        // associations
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public int? PageId { get; set; }
        public virtual Page Page { get; set; }

        public Comment()
        {
            Created = DateTime.UtcNow;
            State = State.Pending;
        }
    }
}
