using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
        public Page Page { get; set; }
        public int? Rating { get; set; }
#if NET45
        public Published Published { get; set; }
#else
        public string Published { get; set; }
#endif

        public Comment()
        {
            CreatedTime = DateTime.UtcNow;
        }
    }
}
