using Instatus.Core;
using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Sample.Controllers
{
    public class PageController : Instatus.Integration.Mvc.PageController
    {
        public PageController(IKeyValueStorage<Document> documents)
            : base(documents)
        {

        }
    }
}
