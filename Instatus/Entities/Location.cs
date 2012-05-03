using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
#if NET45
using System.Data.Spatial;
#endif

namespace Instatus.Entities
{
    [ComplexType]
    public class Location
    {
        public string Name { get; set; }
        public string FormattedAddress { get; set; }
        public string StreetAddress { get; set; }
        public string Locality { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public double ZoomLevel { get; set; }
#if NET45
        public DbGeography Spatial { get; set; }
#else
        public double Latitude { get; set; }
        public double Longitude { get; set; }
#endif
    }
}