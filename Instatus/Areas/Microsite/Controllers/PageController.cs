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
using Instatus.Entities;

namespace Instatus.Areas.Microsite.Controllers
{
    [WebDescriptor(Scope = WebConstant.Scope.Public)]
    [WebParts]
    public class PageController : BaseController
    {
        private IPageModel pageModel;
        private IEnumerable<IContentAdapter> adapters;

        public ActionResult Details(string slug)
        {
            var page = pageModel.GetPage(slug);
            
            foreach(var contentAdapter in adapters) 
            {
                contentAdapter.Process(page, slug);
            }

            ViewData.Model = page;

            return View("Page");
        }

        public PageController(IPageModel pageContext, IEnumerable<IContentAdapter> adapters)
        {
            this.pageModel = pageContext;
            this.adapters = adapters;
        }
    }
}
