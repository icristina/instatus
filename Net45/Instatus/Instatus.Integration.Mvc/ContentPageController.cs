using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    [HttpNotFound]
    public abstract class ContentPageController : Controller
    {
        private IContentManager contentManager;
        
        public ActionResult Details(string key)
        {
            ViewData.Model = contentManager.Get(key);

            return View();
        }

        public ContentPageController(IContentManager contentManager)
        {
            this.contentManager = contentManager;
        }
    }
}
