using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Instatus.Tests
{
    [TestClass]
    public class Paths
    {
        [TestMethod]
        public void SingleFolder()
        {
            var virtualPath = "~/folder/fileName.extension";

            Assert.AreEqual("folder", Path.GetDirectoryName(virtualPath).TrimStart('~', '/', '\\'));
            Assert.AreEqual("fileName.extension", Path.GetFileName(virtualPath));
        }

        [TestMethod]
        public void MultipleFolders()
        {
            var virtualPath = "~/folder1/folder2/fileName.extension";

            Assert.AreEqual("folder1\\folder2", Path.GetDirectoryName(virtualPath).TrimStart('~', '/', '\\'));
            Assert.AreEqual("fileName.extension", Path.GetFileName(virtualPath));
        }
    }
}
