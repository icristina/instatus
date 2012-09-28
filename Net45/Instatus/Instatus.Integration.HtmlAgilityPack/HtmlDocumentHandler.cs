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
    public class HtmlDocumentHandler : IHandler<Document>
    {
        private ITextTemplating textTemplating;
        
        public Document Read(Stream inputStream)
        {
            var htmlDocument = new HtmlDocument();

            htmlDocument.Load(inputStream.ResetPosition());

            var html = htmlDocument.DocumentNode;
            var body = html.Descendants("body");
            var sections = html.Descendants("section");

            var document = new Document()
            {
                Title = html.GetText("title") ?? html.GetText("h1"),
                Metadata = html.Descendants("meta")
                    .ToDictionary(
                        m => m.Attributes["name"].Value, 
                        m => m.Attributes["content"].Value as object)
            };

            if (sections.Any())
            {
                // assumes first child of section is a heading
                document.Sections = sections.Select(section => new Section()
                {
                    Heading = section.GetText("h1") ?? section.GetText("h2"),
                    Body = section.RemoveChild(section.FirstChild).InnerHtml
                })
                .ToArray();
            }
            else
            {
                // if no sections, currently add all as description
                document.Description = body.Any() ? body.First().InnerHtml : html.InnerHtml;
            }

            return document;
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
