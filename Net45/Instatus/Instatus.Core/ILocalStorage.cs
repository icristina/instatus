using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface ILocalStorage
    {
        void Save(string virtualPath, Stream inputStream);
        void Stream(string virtualPath, Stream outputStream);
    }
}
