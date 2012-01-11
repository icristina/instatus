using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Helpers;
using System.IO;

namespace Instatus
{
    public static class WebClientExtensions
    {
        public static dynamic DownloadJson(this WebClient webClient, string uri)
        {
            var response = webClient.DownloadString(uri);
            return Json.Decode(response);
        }

        public static byte[] UploadValues(this WebClient webClient, string uri, object values)
        {
            return webClient.UploadValues(uri, values.ToNameValueCollection(false));
        }

        public static Stream DownloadStream(this WebClient webClient, string uri)
        {
            return webClient.DownloadData(uri).ToStream();
        }
    }
}