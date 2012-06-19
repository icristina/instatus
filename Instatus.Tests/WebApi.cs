using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class WebApi
    {
        [TestMethod]
        public void FormEncoded()
        {
            var hostname = "http://localhost:8080";
            var config = new HttpSelfHostConfiguration(hostname);

            config.Routes.MapHttpRoute(
                "API Default", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();

                var httpClient = new HttpClient();

                var response = httpClient
                                    .PostAsync(hostname + "/api/formdata", new StringContent("value1=a&value2=b"))
                                    .Result
                                    .Content
                                    .ReadAsStringAsync()
                                    .Result;

                Assert.AreEqual("ab", response);
            }
        }
    }

    public class FormDataController : ApiController
    {
        [HttpPost]
        public string GetProductById(string value1, string value2)
        {
            return value1 + value2;
        }
    }
}
