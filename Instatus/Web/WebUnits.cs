using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebUnits
    {
        public static double BytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}