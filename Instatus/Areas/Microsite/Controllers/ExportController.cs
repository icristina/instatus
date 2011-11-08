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
using System.ComponentModel.Composition;

namespace Instatus.Areas.Microsite.Controllers
{
    [Authorize(Roles = "Administrator,Moderator")]
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ExportController : BaseController<BaseDataContext>
    {
        [ImportMany]
        private IEnumerable<IDataExport> exports;

        public ActionResult Index()
        {
            ViewData.Model = new SelectList(exports, "Name", "Name");            
            return View();
        }

        public ActionResult File(string name)
        {
            var dataExport = exports.First(e => e.Name == name);

            Response.ContentType = WebContentType.Csv.ToMimeType();
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.csv", dataExport.Name));

            Generator.SaveCsv(dataExport.Data, Response.OutputStream);

            return new EmptyResult();
        }
    }
}
