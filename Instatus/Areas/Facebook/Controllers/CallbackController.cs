using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Instatus.Data;
using Instatus.Models;
using Instatus.Web;
using Instatus.Controllers;
using Instatus;

namespace Instatus.Areas.Facebook.Controllers
{
    public class CallbackController : BaseController<BaseDataContext>
    {
        [HttpPost]
        public ActionResult Authenticated(string accessToken)
        {
            return Facebook.Authenticated(accessToken).IsEmpty() ?
                ErrorResult() :
                SuccessResult(); 
        }
    }
}
