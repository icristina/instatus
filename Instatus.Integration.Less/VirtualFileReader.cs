using dotless.Core.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Instatus.Integration.Less
{
    // http://jvance.com/blog/2012/08/31/ASPdotNETMVCBundlingOfBootstrapLESSSource.xhtml
    public class VirtualFileReader : IFileReader
    {
        public byte[] GetBinaryFileContents(string fileName)
        {
            fileName = GetFullPath(fileName);
            return File.ReadAllBytes(fileName);
        }

        public string GetFileContents(string fileName)
        {
            fileName = GetFullPath(fileName);
            return File.ReadAllText(fileName);
        }

        public bool DoesFileExist(string fileName)
        {
            fileName = GetFullPath(fileName);
            return File.Exists(fileName);
        }

        private static string GetFullPath(string path)
        {
            return HostingEnvironment.MapPath("~/content/" + path);
        }
    }
}
