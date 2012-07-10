using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace Instatus.Integration.WebApi
{
    public static class ApiControllerExtensions
    {
        public static T AllowTesting<T>(this T apiController) where T : ApiController
        {
            apiController.Request = new HttpRequestMessage();
            apiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            return apiController;
        }
    }
}
