using Instatus.Core;
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
        private IGeocode geocode;
        
        public ActionResult Index(int pageIndex = 0)
        {
            var viewModel = new PagedViewModel<Entry>(Enumerable.Range(1, 1000)
                .Select(s => new Entry(string.Format("Entry {0}", s)))
                .ToList()
                .AsQueryable()
                .OrderBy(e => e.Text), 
                pageIndex, 
                10);

            viewModel.Document = new Document()
            {
                Metadata = new Dictionary<string, object>()
                {
                    { "ipAddress", Request.UserHostAddress },
                    { "country", geocode.GetCountryCode(Request.UserHostAddress) }
                }
            };

            ViewData.Model = viewModel; 
            
            return View();
        }

        public ActionResult NotHere()
        {           
            return new HttpNotFoundResult();
        }

        public ActionResult WindowsApp()
        {
            return View();
        }

        public ActionResult SocialApp()
        {
            return View();
        }

        [OutputCache(Duration = 60)]
        public ActionResult CacheMe()
        {
            return Content(DateTime.UtcNow.ToLongTimeString());
        }

        public HomeController(IGeocode geocode)
        {
            this.geocode = geocode;
        }
    }
}
