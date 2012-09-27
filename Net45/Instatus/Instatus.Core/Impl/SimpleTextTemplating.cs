using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class SimpleTextTemplating : ITextTemplating
    {
        public void Render(string viewName, object viewData, Stream outputStream)
        {
            using (var streamWriter = new StreamWriter(outputStream))
            {
                streamWriter.Write(viewData.ToString());
            }
        }
    }
}
