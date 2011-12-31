using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Helpers;

namespace Instatus
{
    public static class WebClientExtensions
    {
        public static dynamic DownloadJson(this WebClient webClient, string uri)
        {
            var response = webClient.DownloadString(uri);
            return Json.Decode(response);
        }
    }
}