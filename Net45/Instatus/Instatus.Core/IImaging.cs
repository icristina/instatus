using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IImaging
    {
        void Crop(Stream inputStream, Stream outputStream, int left, int top, int width, int height);
        void Resize(Stream inputStream, Stream outputStream, int width, int height, bool mask);
    }
}
