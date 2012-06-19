using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class Facebook
    {
        private FacebookClient CreateClient(int limit = 25)
        {
            var accessToken = ConfigurationManager.AppSettings["FacebookAccessToken"];
            return new FacebookClient(accessToken, limit);
        }
        
        [TestMethod]
        public void Facebook_User()
        {
            var feed = CreateClient().GetGraphApiAsync<FacebookClient.User>("cocacola").Result;

            Assert.AreEqual(feed.name, "Coca-Cola");
        }

        [TestMethod]
        public void Facebook_Posts()
        {
            var count = 50;
            var feed = CreateClient(count).NewsFeed().Result;

            Assert.AreEqual(feed.data.Count, count);
        }
    }
}
