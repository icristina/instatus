using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Controllers;
using Instatus.Models;
using Instatus.Data;
using Instatus.Web;

namespace Instatus.Areas.Google.Controllers
{
    public class GoogleController : BaseController<BaseDataContext>
    {
        public ActionResult RegisterScripts()
        {
            ViewData.Model = Context.GetApplicationCredentials(WebProvider.Google);
            return PartialView();
        }
    }
}
