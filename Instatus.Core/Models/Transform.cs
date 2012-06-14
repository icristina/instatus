using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Models
{
    public class Transform
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Mask { get; set; }

        public Transform() { }

        public Transform(int width, int height, bool mask)
        {
            Width = width;
            Height = height;
            Mask = mask;
        }
    }
}
