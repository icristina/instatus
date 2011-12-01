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
        public ActionResult Index()
        {
            ViewData.Model = SiteMap.RootNode.ChildNodes;
            return View("Sitemap");
        }
        
        public ActionResult Details(string slug = "home")
        {
            return Article(slug);
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

        public ActionResult Brand()
        {
            ViewData.Model = Context.GetCurrentBrand();
            return PartialView();
        }

        public ActionResult Legal()
        {
            var query = new WebQuery()
            {
                Category = "Legal"
            };

            var brand = Context.GetCurrentBrand();

            ViewData.Model = new WebView<Article>(Context.GetPages<Article>(query), query)
            {
                Name = brand.Name
            };

            return PartialView();
        }
    }
}
