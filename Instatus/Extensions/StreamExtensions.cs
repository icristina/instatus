using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Instatus
{
    public static class StreamExtensions
    {
        public static string CopyToString(this Stream stream)
        {
            stream.Position = 0;

            var input = new StreamReader(stream).ReadToEnd();

            stream.Position = 0;

            return input;
        }
    }
}