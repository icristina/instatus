using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Media : Link
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int Duration { get; set; }
        public string Embed { get; set; }
    }
}
