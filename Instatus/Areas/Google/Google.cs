using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using System.Net;
using System.Text;
using System.Globalization;

namespace Instatus.Areas.Google
{
    public static class Google
    {
        public static string StaticMap(double latitude, double longitude, int zoom = 15, int width = 100, int height = 100)
        {
            return string.Format("https://maps.googleapis.com/maps/api/staticmap?center={0},{1}&zoom={2}&size={3}x{4}&sensor=false", latitude, longitude, zoom, height, width);
        }
    }
}