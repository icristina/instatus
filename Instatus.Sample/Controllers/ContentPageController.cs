using Instatus.Core;
using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Sample.Controllers
{
    public class ContentPageController : Instatus.Integration.Mvc.ContentPageController
    {
        public ContentPageController(IKeyValueStorage<Document> documents)
            : base(documents)
        {

        }
    }
}
