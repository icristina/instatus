using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace Instatus.Integration.Mvc
{
    public class RssResult : ActionResult
    {
        private SyndicationFeed syndicationFeed;
        
        public override void ExecuteResult(ControllerContext context)
        {
            var formatter = new Rss20FeedFormatter(syndicationFeed);
            var response = context.HttpContext.Response;
            
            response.ContentType = WellKnown.ContentType.Rss;

            using (var xmlWriter = XmlWriter.Create(response.Output))
            {
                formatter.WriteTo(xmlWriter);
            }
        }

        public RssResult(SyndicationFeed syndicationFeed)
        {
            this.syndicationFeed = syndicationFeed;
        }
    }
}
