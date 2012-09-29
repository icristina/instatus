using Instatus.Core;
using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Instatus.Integration.Mvc
{
    [HttpNotFound]
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class ContentPageController : Controller
    {
        private IKeyValueStorage<Document> documents;
        
        public ActionResult Details(string key)
        {
            ViewData.Model = documents.Get(key);

            return View();
        }

        public ContentPageController(IKeyValueStorage<Document> contentManager)
        {
            this.documents = contentManager;
        }
    }
}
