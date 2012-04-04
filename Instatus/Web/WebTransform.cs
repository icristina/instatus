using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebTransform
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Mask { get; set; }

        public WebTransform()
        {

        }

        public WebTransform(int width, int height, bool mask)
        {
            Width = width;
            Height = height;
            Mask = mask;
        }
    }
}