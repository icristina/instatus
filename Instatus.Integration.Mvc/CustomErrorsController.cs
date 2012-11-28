using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Instatus.Integration.Mvc
{
    public class CustomErrorsController : Controller
    {
        public ActionResult Error()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;                
            
            return View();
        }        
        
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;            
            
            return View();
        }
    }
}
