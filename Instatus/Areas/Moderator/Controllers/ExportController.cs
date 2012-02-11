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
using System.Data.Entity;
using System.ComponentModel;

namespace Instatus.Areas.Moderator.Controllers
{
    [Authorize(Roles = "Moderator")]
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Description("Export")]
    public class ExportController : ScaffoldController<BaseViewModel<WebEntry>, WebEntry, WebEntryRepository, string>
    {
        [ImportMany]
        private IEnumerable<IDataExport> exports;

        public override IEnumerable<WebEntry> Query(IEnumerable<WebEntry> set, WebQuery query)
        {
            return exports.Select(e => new WebEntry()
            {
                Title = e.Name
            });
        }

        public override void ConfigureWebView(WebView<WebEntry> webView)
        {
            base.ConfigureWebView(webView);
            webView.Permissions = new string[] { };
        }

        public override ICollection<IWebCommand> GetCommands(WebQuery query)
        {
            return new List<IWebCommand>()
            {
                new ExportCommand()
            };
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

    public class ExportCommand : IWebCommand
    {
        public string Name
        {
            get {
                return "ExportCommand";
            }
        }

        public WebLink GetLink(dynamic viewModel, UrlHelper urlHelper)
        {
            return new WebLink()
            {
                Title = "Export",
                Uri = urlHelper.Action("File", new { name = viewModel.Title  })
            };
        }

        public bool Execute(dynamic viewModel, UrlHelper urlHelper, System.Web.Routing.RouteData routeData, System.Collections.Specialized.NameValueCollection requestParams)
        {
            return true;
        }
    }

    public class WebEntryRepository : IRepository<WebEntry>{
        public IDbSet<WebEntry> Items
        {
	        get {
                return new WebEntrySet();
            }
        }

        public int SaveChanges()
        {
 	        return 0;
        }

        public void  Dispose()
        {

        }
    }

    internal class WebEntrySet : InMemorySet<WebEntry>
    {

    }
}
