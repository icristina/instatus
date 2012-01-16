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
    public class FacebookController : BaseController<BaseDataContext>
    {
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult RegisterScripts()
        {
            ViewData.Model = Context.GetApplicationCredential(WebProvider.Facebook);
            return PartialViewOrEmpty();
        }

        [HttpPost]
        public ActionResult Authenticated(string accessToken)
        {
            return Facebook.Authenticated(accessToken).IsEmpty() ?
                ErrorResult() :
                SuccessResult();
        }

        public ActionResult Channel()
        {
            var protocol = Request.Url.Scheme;
            var html = string.Format("<script src='{0}://connect.facebook.net/en_US/all.js'></script>", protocol);
            return Content(html);
        }
    }
}
