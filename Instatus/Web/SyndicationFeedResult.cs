using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Instatus.Web
{
    public class SyndicationFeedResult : ActionResult
    {
        public WebContentType Format { get; set; }
        public SyndicationFeed Feed { get; set; }

        public SyndicationFeedResult(WebContentType format, SyndicationFeed feed)
        {
            Format = format;
            Feed = feed;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            SyndicationFeedFormatter formatter;

            switch (Format)
            {
                case WebContentType.Atom:
                    formatter = Feed.GetAtom10Formatter();
                    break;
                case WebContentType.Rss:
                    formatter = Feed.GetRss20Formatter();
                    break;
                default:
                    throw new Exception("Invalid content type");
            }

            response.ContentType = Format.ToMimeType();

            using (var xmlWriter = new XmlTextWriter(response.Output))
            {
                xmlWriter.Formatting = Formatting.Indented;
                formatter.WriteTo(xmlWriter);
            }
        }
    }
}