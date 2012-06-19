using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
                "API Default", "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();

                var httpClient = new HttpClient();

                var formData = new Dictionary<string, string>()
                {
                    { "value1", "a" },
                    { "value2", "b" }
                };

                var response = httpClient
                                    .PostAsync(hostname + "/api/formdata/complextype", new FormUrlEncodedContent(formData))
                                    .Result
                                    .Content
                                    .ReadAsStringAsync()
                                    .Result;

                Assert.AreEqual("\"ab\"", response);
            }
        }
    }

    public class FormDataController : ApiController
    {
        public class FormData
        {
            public string value1 { get; set; }
            public string value2 { get; set; }
        }
        
        [HttpPost]
        public string SimpleTypes(string value1, string value2)
        {
            return value1 + value2;
        }

        [HttpPost]
        public string ComplexType(FormData formData)
        {
            return formData.value1 + formData.value2;
        }
    }
}
