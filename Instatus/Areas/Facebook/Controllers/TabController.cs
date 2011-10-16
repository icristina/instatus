using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Controllers;
using Instatus.Models;
using Instatus.Data;
using Instatus.Web;

namespace Instatus.Areas.Facebook.Controllers
{
    public class TabController : BaseController<BaseDataContext>
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return Article();
        }

        public ActionResult Terms()
        {
            return Article();
        }
    }
}
