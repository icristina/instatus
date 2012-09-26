using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface IDocumentHandler
    {
        Document Parse(Stream inputStream);
        void Write(Document document, Stream outputStream);
        string FileExtension { get; }
    }
}
