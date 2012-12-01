using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;

namespace Instatus.Integration.Mvc
{
    [Authorize(Roles = "Administrator")]
    [SessionState(SessionStateBehavior.Disabled)]
    public class ClearCacheController : Controller
    {
        public ActionResult Index(string returnUrl)
        {
            Response.RemoveOutputCacheItem(returnUrl);
            return Redirect(returnUrl);
        }
    }
}
