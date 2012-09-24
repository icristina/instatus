using Instatus.Core;
using MaxMind.GeoIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Instatus.Integration.Maxmind
{
    public class DataFileGeocode : IGeocode
    {
        public string GetCountryCode(string ipAddress)
        {
            var virtualPath = "~/App_Data/GeoIP.dat";
            var absolutePath = HostingEnvironment.MapPath(virtualPath);
            var lookupService = new LookupService(absolutePath, LookupService.GEOIP_MEMORY_CACHE);
            var countryCode = lookupService.getCountry(ipAddress).getCode();

            return countryCode != "--" ? countryCode : null;
        }
    }
}
