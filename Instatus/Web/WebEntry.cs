using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class WebGeospatialEntry : WebEntry
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
    
    public class WebEntry
    {
        public string Kind { get; set; }
        public string Uri { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Caption { get; set; }
        public string User { get; set; }
        public string Source { get; set; }
        public DateTime Timestamp { get; set; }
    }
}