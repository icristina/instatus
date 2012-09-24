using Instatus.Core;
using Instatus.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Instatus.Integration.Maxmind
{
    // http://dev.maxmind.com/geoip/web-services
    public class MaxmindGeocode : IGeocode
    {
        private ICredentialStorage credentialStorage;

        private const string baseAddress = "http://geoip.maxmind.com";
        
        public string GetCountryCode(string ipAddress)
        {
            var webServiceUri = new PathBuilder(baseAddress)
                .Path("a")
                .Query("l", credentialStorage.GetCredential(WellKnown.Provider.Maxmind).PrivateKey)
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

        public MaxmindGeocode(ICredentialStorage credentialStorage)
        {
            this.credentialStorage = credentialStorage;
        }
    }
}
