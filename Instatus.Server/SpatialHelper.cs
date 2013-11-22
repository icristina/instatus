using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Server
{
    public static class SpatialHelper
    {
        public static DbGeography FromLatLong(double latitude, double longitude)
        {
            return DbGeography.FromText(string.Format("POINT({0} {1})", latitude, longitude));
        }
    }
}
