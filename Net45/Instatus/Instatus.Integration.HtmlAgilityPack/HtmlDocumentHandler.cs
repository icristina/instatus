using Instatus.Core;
using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Instatus.Core.Extensions;
using HtmlAgilityPack;

namespace Instatus.Integration.HtmlAgilityPack
{
    public class HtmlDocumentHandler : IDocumentHandler
    {
        public Document Parse(Stream inputStream)
        {
            var htmlDocument = new HtmlDocument();

            inputStream.ResetPosition();

            htmlDocument.Load(inputStream);

            return new Document()
            {
                Title = htmlDocument.DocumentNode.Descendants("h1").First().InnerText,
                Description = htmlDocument.DocumentNode.Descendants("body").First().InnerHtml
            };
        }

        public void Write(Document document, Stream outputStream)
        {
            throw new NotImplementedException();
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
