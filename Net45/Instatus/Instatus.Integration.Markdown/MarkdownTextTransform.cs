using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Instatus.Core;

namespace Instatus.Integration.Markdown
{
    public class MarkdownTextTransform : ITextTransform
    {
        public string Transform(string text)
        {
            var markdown = new MarkdownSharp.Markdown();
            return markdown.Transform(text);
        }
    }
}
