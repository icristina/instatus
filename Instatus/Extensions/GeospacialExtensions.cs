using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus
{
    public static class GeospacialExtensions
    {
        public enum DistanceUnit
        {
            Kilometers,
            Miles
        }

        // http://www.movable-type.co.uk/scripts/latlong.html
        // http://www.zipcodeworld.com/samples/distance.cs.html
        // http://megocode3.wordpress.com/2008/02/05/haversine-formula-in-c/
        public static double SphericalDistance(double lat1, double lon1, double lat2, double lon2, DistanceUnit type = DistanceUnit.Kilometers)
        {
            double R = (type == DistanceUnit.Miles) ? 3960 : 6371;

            double dLat = (lat2 - lat1).ToRadian();
            double dLon = (lon2 - lon1).ToRadian();
 
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1.ToRadian()) *Math.Cos(lat2.ToRadian()) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
 
            return d;
        }

        private static double ToRadian(this double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}