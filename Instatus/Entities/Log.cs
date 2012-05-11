using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public string Uri { get; set; }
        public string Message { get; set; }
        public DateTime CreatedTime { get; set; }

        public Log()
        {
            CreatedTime = DateTime.UtcNow;
        }
    }
}
