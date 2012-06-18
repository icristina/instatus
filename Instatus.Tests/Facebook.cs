using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class Facebook
    {
        [TestMethod]
        public void Facebook_User()
        {
            var accessToken = ConfigurationManager.AppSettings["FacebookAccessToken"];
            var facebookClient = new FacebookClient(accessToken);
            var feed = facebookClient.GetGraphApiAsync<FacebookClient.User>("cocacola").Result;

            Assert.AreEqual(feed.name, "Coca-Cola");
        }
    }
}
