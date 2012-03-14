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

namespace Instatus.Areas.Developer.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DashboardController : BaseController<DbApplicationContext>
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadPages(string path)
        {
            using (var stream = GetStream(path))
            {
                Context.LoadPages(stream);
            }

            SetResult("Loaded Pages");

            return View("Index");
        }

        public ActionResult LoadParts(string path)
        {
            using (var stream = GetStream(path))
            {
                WebPart.Catalog.Clear();
                WebPart.Catalog.AddRange(Generator.LoadXml<List<WebPart>>(stream));
            }

            SetResult("Loaded Parts");

            return View("Index");
        }

        private void SetResult(string message)
        {
            ViewData.Model = new List<WebParameter>()
            {
                new WebParameter(message, DateTime.Now.ToString())
            };
        }

        private Stream GetStream(string path)
        {
            if (Request.HasFile())
                return Request.FileInputStream();

            if (!path.IsEmpty())
                return new FileSystemBlobService().Stream(path);

            return null;
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
            WebApp.Reset();

            SetResult("Application Reset");
            
            return View("Index");
        }

        public ActionResult RequestParams()
        {
            var parameters = new List<WebParameter>();

            foreach (var key in Request.Params.AllKeys)
                parameters.Add(new WebParameter(key, Request.Params[key]));

            ViewData.Model = parameters;
            
            return View("Index");
        }
    }
}
