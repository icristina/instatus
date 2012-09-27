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
        private ITextTemplating textTemplating;
        
        public Document Parse(Stream inputStream)
        {
            var htmlDocument = new HtmlDocument();

            htmlDocument.Load(inputStream.ResetPosition());

            var html = htmlDocument.DocumentNode;

            return new Document()
            {
                Title = (html.Descendants("title").FirstOrDefault() ?? html.Descendants("h1").First()).InnerText,
                Description = html.Descendants("body").First().InnerHtml,
                Metadata = html.Descendants("meta").ToDictionary(m => m.Attributes["name"].Value, m => m.Attributes["content"].Value as object)
            };
        }

        public void Write(Document document, Stream outputStream)
        {
            textTemplating.Render("Document", document, outputStream);
        }

        public string FileExtension
        {
            get 
            {
                return "html";
            }
        }

        public HtmlDocumentHandler(ITextTemplating textTemplating)
        {
            this.textTemplating = textTemplating;
        }    
    }
}
