using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using Instatus.Data;

namespace Instatus.Adapters
{
    public interface IContentAdapter
    {
        void Process(IContentItem contentItem, IContentRepository contentRepository, string hint);
    }
}