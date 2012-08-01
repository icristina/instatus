using System;
using System.IO;
using Instatus.Core.Impl;
using Instatus.Integration.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Instatus.Tests
{
    [TestClass]
    public class FileSystem
    {
        [TestMethod]
        public void Query()
        {
            var binDebugFolder = AppDomain.CurrentDomain.BaseDirectory;
            var projectFolder = new DirectoryInfo(binDebugFolder).Parent.Parent.FullName;
            
            var hostingEnvironment = new InMemoryHostingEnvironment(null)
            {
                RootPath =  projectFolder
            };

            var fileSystemBlobStorage = new FileSystemBlobStorage(hostingEnvironment);
            var files = fileSystemBlobStorage.Query("~/Properties");

            Assert.IsTrue(files[0].Contains("AssemblyInfo.cs"));
        }
    }
}
