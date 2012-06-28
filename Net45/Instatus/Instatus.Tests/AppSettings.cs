using System;
using Instatus.Integration.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class AppSettings
    {
        [TestMethod]
        public void ParseCredentialFromString()
        {
            var appSetting = "AccountName=a;PrivateKey=b==";
            var hostingEnvironment = new AspNetHostingEnvironment();
            var credentialStorage = new AppSettingsCredentialStorage(hostingEnvironment);
            var values = credentialStorage.ParseDelimitedString(appSetting);
            var credential = credentialStorage.ConvertToCredential(values);

            Assert.AreEqual("a", credential.AccountName);
            Assert.AreEqual("b==", credential.PrivateKey);
        }
    }
}
