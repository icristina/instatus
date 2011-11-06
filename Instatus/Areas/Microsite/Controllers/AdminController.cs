using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Models;
using Instatus.Data;
using Instatus.Controllers;
using Instatus.Web;
using Instatus.Services;
using System.IO;

namespace Instatus.Areas.Microsite.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : BaseController<BaseDataContext>
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadPages(string path)
        {
            Stream stream = null;
            
            if (Request.HasFile())
            {
                stream = Request.FileInputStream();
            }
            else if (!path.IsEmpty())
            {
                stream = new LocalStorageBlobService().Stream(path);
            }

            Context.LoadPages(stream);
            
            return RedirectToIndex();
        }
    }
}
