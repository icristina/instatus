using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using Instatus.Data;
using Instatus.Models;
using System.ComponentModel.Composition;

namespace Instatus.Adapters
{
    // process WebIncludes
    [Export(typeof(IContentAdapter))]
    [PartCreationPolicy(CreationPolicy.NonShared)]   
    public class IncludeAdapter : IContentAdapter
    {
        public void Process(IContentItem contentItem, IContentRepository contentRepository, string hint)
        {
            if (contentItem.Document != null)
            {
                foreach (var include in contentItem.Document.Parts.OfType<WebInclude>().ToList())
                {
                    contentItem.Document.Parts.Remove(include);

                    var childPage = contentRepository.GetPage(include.Uri);

                    if (childPage.Document == null)
                        break;

                    if (childPage.Document.Body != null)
                        contentItem.Document.Parts.Add(new WebSection()
                        {
                            Heading = childPage.Document.Title,
                            Abstract = childPage.Document.Description,
                            Body = childPage.Document.Body
                        });

                    if (childPage.Document.Parts != null)
                        contentItem.Document.Parts.Append(childPage.Document.Parts);
                }
            }
        }
    }
}