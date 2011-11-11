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

        public ActionResult GenerateHash(string value, string salt)
        {
            ViewData.Model = new List<WebParameter>()
            {
                new WebParameter() {
                    Name = "Value",
                    Content = value
                },
                new WebParameter() {
                    Name = "Secret",
                    Content = salt
                },
                new WebParameter() {
                    Name = "Hash",
                    Content = value.ToEncrypted(salt)
                }
            };

            return View("Index");
        }

        public ActionResult GenerateUnixTimestamp(DateTime date)
        {
            ViewData.Model = new List<WebParameter>()
            {
                new WebParameter() {
                    Name = "Date",
                    Content = date.ToString()
                },
                new WebParameter() {
                    Name = "Unix Timestamp",
                    Content = date.ToUnixTimestamp().ToString()
                }
            };

            return View("Index");
        }

        public ActionResult ThrowError(string message)
        {
            throw new Exception(message);
        }
    }
}
