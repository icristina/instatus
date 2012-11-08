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
    public abstract class PageController : Controller
    {
        private IKeyValueStorage<Document> documents;
        private IPreferences preferences;
        
        public ActionResult Details(string key)
        {
            ViewData.Model = documents.Get(preferences.Locale, key) ?? documents.Get(null, key);

            return View();
        }

        public PageController(IKeyValueStorage<Document> contentManager, IPreferences preferences)
        {
            this.documents = contentManager;
            this.preferences = preferences;
        }
    }
}
