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
        Stream OpenWrite(string virtualPath);
        Stream OpenRead(string virtualPath);
        void Delete(string virtualPath);
        string[] Query(string virtualPath, string suffix);
    }
}
