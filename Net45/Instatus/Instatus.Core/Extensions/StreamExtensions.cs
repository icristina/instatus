using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Instatus.Core.Extensions
{
    public static class StreamExtensions
    {
        public static Stream ResetPosition(this Stream stream)
        {
            stream.Position = 0;
            return stream;
        }
    }
}
