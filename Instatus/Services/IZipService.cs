using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Instatus.Services
{
    public interface IZipService
    {
        bool FileExists(string packagePath, string relativePath);
        Stream OpenStream(string packagePath, string relativePath);
    }
}
