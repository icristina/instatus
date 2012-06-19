using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Entities
{
    public class Location
    {
        public string Name { get; set; }
        public string FormattedAddress { get; set; }
        public string StreetAddress { get; set; }
        public string Locality { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public double ZoomLevel { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}