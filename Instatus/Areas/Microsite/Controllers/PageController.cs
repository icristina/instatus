using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus;
using Instatus.Models;
using Instatus.Data;
using Instatus.Controllers;
using Instatus.Web;
using System.ComponentModel.Composition;

namespace Instatus.Areas.Microsite.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [WebDescriptor(Scope = WebConstant.Scope.Public)]
    [WebParts]
    public class PageController : BaseController
    {
        private IPageContext pageContext;

        public ActionResult Details(string slug)
        {
            ViewData.Model = pageContext.GetPage(slug).ApplyAdapters();
            return View("Page");
        }

        [ImportingConstructor]
        public PageController(IPageContext pageContext)
        {
            this.pageContext = pageContext;
        }
    }
}
