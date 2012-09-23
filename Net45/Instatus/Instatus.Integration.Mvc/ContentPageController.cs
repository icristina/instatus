using Instatus.Core;
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
