using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Web
{
    public class WebCatalog
    {
        public static Dictionary<ImageSize, Transform> ImageSizes = new Dictionary<ImageSize, Transform>();
        public static IList<Part> Parts = new List<Part>();
    }
}