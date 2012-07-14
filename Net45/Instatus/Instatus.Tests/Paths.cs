using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Instatus.Core.Utils;

namespace Instatus.Tests
{
    [TestClass]
    public class Paths
    {
        [TestMethod]
        public void BuildPathWithQuery()
        {
            var pathBuilder = new PathBuilder("http://www.google.com/")
                .Path("/search/")
                .Query("q", "dotnet")
                .Query("fields", new string[] { "a", "b" })
                .Query("prop", null);

            Assert.AreEqual("http://www.google.com/search?q=dotnet&fields=a%2cb", pathBuilder.ToString());
        }

        [TestMethod]
        public void ProtocolRelative()
        {
            var pathBuilder = new PathBuilder("http://www.google.com/")
                .Path("/search/")
                .Query("q", "dotnet");

            Assert.AreEqual("//www.google.com/search?q=dotnet", pathBuilder.ToProtocolRelativeUri());
        }

        [TestMethod]
        public void LowerCasePaths()
        {
            var pathBuilder1 = new PathBuilder("http://Google.com/Search/", true);
            var pathBuilder2 = new PathBuilder("http://Google.com/Search/?Query=1", true);

            Assert.AreEqual("http://google.com/search", pathBuilder1.ToString(), false);
            Assert.AreEqual("http://google.com/search?Query=1", pathBuilder2.ToString(), false);
        }
        
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
