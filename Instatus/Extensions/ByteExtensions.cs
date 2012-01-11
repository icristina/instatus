using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Helpers;

namespace Instatus
{
    public static class ByteExtensions
    {
        public static Stream ToStream(this WebImage image)
        {
            return image.GetBytes().ToStream();
        }
        
        public static Stream ToStream(this byte[] bytes) {
            return new MemoryStream(bytes);
        }

        public static string ToStringValue(this byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes);
        }
    }
}