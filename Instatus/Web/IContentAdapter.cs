using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Web;
using Instatus.Data;
using Instatus.Models;

namespace Instatus.Web
{
    public interface IContentAdapter
    {
        void Process(IContentItem contentItem, string hint);
    }
}