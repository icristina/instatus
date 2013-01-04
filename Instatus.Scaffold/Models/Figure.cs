using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Models
{
    public class Figure
    {
        public string Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool EnableResizeToCover { get; set; } // true = cover (fill height and width); false = contain in bounding box
    }
}