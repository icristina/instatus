using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Instatus.Core;

namespace Instatus.Integration.Markdown
{
    public class MarkdownTextTemplating : ITextTemplating
    {
        public void Render(string viewName, object viewData, Stream outputStream)
        {
            var content = viewData.ToString();
            var markdown = new MarkdownSharp.Markdown();
            var output = markdown.Transform(content);

            using (var streamWriter = new StreamWriter(outputStream))
            {
                streamWriter.Write(output);
            }
        }
    }
}
