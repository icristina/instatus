using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Models;
using Instatus.Data;
using Instatus.Controllers;
using Instatus.Web;

namespace Instatus.Areas.Microsite.Controllers
{
    public class ArticleController : BaseController<BaseDataContext>
    {
        public ActionResult Details(string slug = "home")
        {
            ViewData.Model = Context.GetPage<Article>(slug);
            return View("Article");
        }

        public ActionResult Stream(WebQuery query, string viewName = "")
        {
            ViewData.Model = new WebView<Page>(Context.GetPages(query), query);
            return PartialView(viewName);
        }

        public ActionResult Nav()
        {
            ViewData.Model = SiteMap.RootNode.ChildNodes;
            return PartialView();
        }
    }
}
