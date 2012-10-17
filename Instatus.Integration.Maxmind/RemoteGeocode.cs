using Instatus.Core;
using Instatus.Core.Models;
using Instatus.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Instatus.Integration.Maxmind
{
    // http://dev.maxmind.com/geoip/web-services
    public class RemoteGeocode : IGeocode
    {
        private IKeyValueStorage<Credential> credentials;

        private const string baseAddress = "http://geoip.maxmind.com";
        
        public string GetCountryCode(string ipAddress)
        {
            var webServiceUri = new PathBuilder(baseAddress)
                .Path("a")
                .Query("l", credentials.Get(WellKnown.Provider.Maxmind).PrivateKey)
                .Query("i", ipAddress)
                .ToString();

            using (var webClient = new WebClient())
            {
                try
                {
                    var responseString = webClient.DownloadString(webServiceUri);
                    return responseString.Split(',')[0];
                }
                catch
                {
                    return null;
                }
            }
        }

        public RemoteGeocode(IKeyValueStorage<Credential> credentials)
        {
            this.credentials = credentials;
        }
    }
}
