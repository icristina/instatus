using Instatus.Core.Models;
using Instatus.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Sample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int pageIndex = 0)
        {
            ViewData.Model = new PagedViewModel<Entry>(Enumerable.Range(1, 1000)
                .Select(s => new Entry(string.Format("Entry {0}", s)))
                .ToList()
                .AsQueryable()
                .OrderBy(e => e.Text), 
                pageIndex, 
                10);
            
            return View();
        }
    }
}
