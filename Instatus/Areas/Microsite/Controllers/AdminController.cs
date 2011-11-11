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

            ViewData.Model = new List<WebParameter>()
            {
                new WebParameter("Loaded", DateTime.Now.ToString())
            };

            return View("Index");
        }

        public ActionResult GenerateHash(string value, string salt)
        {
            ViewData.Model = new List<WebParameter>()
            {
                new WebParameter("Value", value),
                new WebParameter("Secret", salt),
                new WebParameter("Hash", value.ToEncrypted(salt))
            };

            return View("Index");
        }

        public ActionResult GenerateUnixTimestamp(DateTime date)
        {
            ViewData.Model = new List<WebParameter>()
            {
                new WebParameter("Date", date.ToString()),
                new WebParameter("Unix Timestamp", date.ToUnixTimestamp().ToString())
            };

            return View("Index");
        }

        public ActionResult ThrowError(string message)
        {
            throw new Exception(message);
        }

        public ActionResult ApplicationReset()
        {
            PubSub.Provider.Publish(new ApplicationReset());
            
            ViewData.Model = new List<WebParameter>()
            {
                new WebParameter("Refreshed", DateTime.Now.ToString())
            };
            
            return View("Index");
        }
    }
}
