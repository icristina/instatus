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
    // add WebDocument and optionally name and description to a Page or WebView based on slug
    [Export(typeof(IContentAdapter))]
    [PartCreationPolicy(CreationPolicy.NonShared)]    
    public class DocumentAdapter : IContentAdapter
    {
        public void Process(IContentItem contentItem, IContentRepository contentRepository, string hint)
        {
            if (!hint.IsEmpty())
            {
                var page = GetPage(contentRepository, hint);

                if (page != null)
                {
                    AttachPage(contentItem, page);
                }
            }
        }

        public virtual Page GetPage(IContentRepository contentRepository, string hint)
        {
            return contentRepository.GetPage(hint);
        }

        public virtual void AttachPage(IContentItem contentItem, Page page)
        {
            contentItem.Name = page.Name;
            contentItem.Document = page.Document;

            if (contentItem.Document != null && !page.Description.IsEmpty() && contentItem.Document.Description.IsEmpty())
            {
                contentItem.Document.Description = page.Description;
            }
        }
    }
}