using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Controllers;
using System.Web.Mvc;

namespace Instatus.Areas.Facebook.Controllers
{
    public class ChannelController : BaseController
    {
        public ActionResult Index()
        {
            var protocol = Request.Url.Scheme;
            var html = string.Format("<script src='{0}://connect.facebook.net/en_US/all.js'></script>", protocol);
            return Content(html);
        }
    }
}