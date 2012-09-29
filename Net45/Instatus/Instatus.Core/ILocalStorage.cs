using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface ILocalStorage : IVirtualPathUtility
    {
        void Save(string virtualPath, Stream inputStream);
        void Stream(string virtualPath, Stream outputStream);
        void Delete(string virtualPath);
        string[] Query(string virtualPath, string suffix);
    }
}
