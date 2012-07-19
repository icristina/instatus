using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Sample.Models;

namespace Instatus.Sample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Subscribe()
        {
            ViewData.Model = new SubscribeModel();

            return View("Create");
        }

        [HttpPost]
        public ActionResult Subscribe(SubscribeModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            ViewData.Model = viewModel;

            return View("Create");
        }
    }
}
