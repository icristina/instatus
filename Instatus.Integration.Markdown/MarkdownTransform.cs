using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Instatus.Core;

namespace Instatus.Integration.Markdown
{
    public class MarkdownTransform : ITransform<string>
    {
        public string Transform(string text)
        {
            var markdown = new MarkdownSharp.Markdown();
            return markdown.Transform(text);
        }
    }
}
