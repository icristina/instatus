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
    public class PageController : BaseController<BaseDataContext>
    {
        public ActionResult Index()
        {
            return RedirectToAction("Details");
        }
        
        public ActionResult Details(string slug = WebRoute.HomeSlug)
        {
            return Page(slug);
        }

        public ActionResult Stream(WebQuery query, string viewName = "")
        {
            ViewData.Model = GetStream(query);
            return PartialView(viewName);
        }

        private object GetStream(WebQuery query)
        {
            switch (query.Kind)
            {
                case WebKind.Link:
                    return new WebView<Link>(Context.GetLinks(query), query);
                case WebKind.User:
                    return new WebView<User>(Context.GetUsers(query), query);
                default:
                    return new WebView<Page>(Context.GetPages(query), query);
            }
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
            var query = new WebQuery();

            var brand = Context.GetCurrentBrand();

            ViewData.Model = new WebView<Article>(Context.GetPages<Article>(query, category: "Legal"), query)
            {
                Name = brand.Name
            };

            return PartialView();
        }


        public ActionResult Sitemap()
        {
            ViewData.Model = SiteMap.RootNode.ChildNodes;
            return View();
        }
    }
}
