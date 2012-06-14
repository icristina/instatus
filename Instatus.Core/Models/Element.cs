using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Element
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public int Left { get; set; } // x coordinate
        public int Top { get; set; } // y coordinate
        public int Width { get; set; }
        public int Height { get; set; }
        public string Category { get; set; }
        public int ZIndex { get; set; }
        public double Weighting { get; set; } // priority or accuracy
    }
}
