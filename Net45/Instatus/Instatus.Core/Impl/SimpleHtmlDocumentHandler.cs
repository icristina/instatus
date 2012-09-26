using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Instatus.Core.Extensions;

namespace Instatus.Core.Impl
{
    public class SimpleHtmlDocumentHandler : IDocumentHandler
    {
        public Document Parse(Stream inputStream)
        {
            return new Document()
            {
                Description = inputStream.ReadAsString()
            };
        }

        public void Write(Document document, Stream outputStream)
        {
            using (var streamWriter = new StreamWriter(outputStream))
            {
                streamWriter.Write(document.Description);
            }
        }

        public string FileExtension
        {
            get 
            {
                return "html";
            }
        }
    }
}
