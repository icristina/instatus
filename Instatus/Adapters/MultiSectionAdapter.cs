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
    // split Document.Body into multiple sections
    [Export(typeof(IContentAdapter))]
    [PartCreationPolicy(CreationPolicy.NonShared)]   
    public class MultiSectionAdapter : IContentAdapter
    {
        public void Process(IContentItem contentItem, IContentRepository contentRepository, string hint)
        {
            var document = contentItem.Document;
            var body = document.Body;

            foreach (var section in body.ToLabelledList())
            {
                document.Parts.Add(new WebSection()
                {
                    Heading = section.Item1,
                    Body = section.Item2
                });
            }

            document.Body = null;            
        }
    }
}