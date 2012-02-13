using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Controllers;
using Instatus.Models;
using Instatus.Data;
using Instatus.Web;

namespace Instatus.Areas.Typekit.Controllers
{
    public class TypekitController : BaseController<IBaseDataContext>
    {
        public ActionResult RegisterScripts()
        {
            ViewData.Model = Context.GetApplicationCredential(WebProvider.Typekit);
            return PartialViewOrEmpty();
        }
    }
}
