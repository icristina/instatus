using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Text { get; set; }

        public Log()
        {
            Created = DateTime.UtcNow;
        }
    }
}
