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
using System.Data;

namespace Instatus.Areas.Moderator.Controllers
{
    [Authorize(Roles = "Moderator")]
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Description("Export")]
    public class ExportController : ScaffoldController<BaseViewModel<WebEntry>, WebEntry, IDbSet<WebEntry>, string>
    {
        [ImportMany]
        private IEnumerable<IDataExport> exports;

        public override IEnumerable<WebEntry> Query(IEnumerable<WebEntry> set, WebQuery query)
        {
            return exports.Select(e => new WebEntry()
            {
                Title = e.Name,
                Rel = "Configurable"
            });
        }

        public ActionResult Configure(string name)
        {
            var dataExport = exports.FirstByName(name);
            var configuration = dataExport.DefaultConfiguration;

            configuration.TryDatabind();

            ViewData.Model = configuration;

            ViewData.AddSingle(new WebForm()
            {
                ActionName = "Download",
                ActionText = "Export",
                HiddenParameters = new List<WebParameter>()
                {
                    new WebParameter("name", name)
                }
            });

            return View("Edit");
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
                new ExportCommand(),
                new ConfigureCommand()
            };
        }

        public ActionResult Download(string name)
        {
            var dataExport = exports.FirstByName(name);
            var configuration = dataExport.DefaultConfiguration;

            if (configuration != null)
            {
                UpdateModel(configuration);
            }

            Response.ContentType = WebContentType.Csv.ToMimeType();
            Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.csv", dataExport.Name));

            var enumerable = dataExport.Export(configuration);

            if (enumerable is DataRowCollection) {
                var dataRows = enumerable as DataRowCollection;

                if (dataRows.Count > 0) {
                    Generator.SaveCsv(dataRows[0].Table, Response.OutputStream);
                }
            } else {
                Generator.SaveCsv(enumerable, Response.OutputStream);
            }

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
                Uri = urlHelper.Action("Download", new { name = viewModel.Title  })
            };
        }

        public bool Execute(dynamic viewModel, UrlHelper urlHelper, System.Web.Routing.RouteData routeData, System.Collections.Specialized.NameValueCollection requestParams)
        {
            return true;
        }
    }

    public class ConfigureCommand : IWebCommand
    {
        public string Name
        {
            get
            {
                return "ConfigureCommand";
            }
        }

        public WebLink GetLink(dynamic viewModel, UrlHelper urlHelper)
        {
            if (viewModel.Rel == "Configurable")
            {
                return new WebLink()
                {
                    Title = "Configure",
                    Uri = urlHelper.Action("Configure", new { name = viewModel.Title })
                };
            }
            else
            {
                return null;
            }
        }

        public bool Execute(dynamic viewModel, UrlHelper urlHelper, System.Web.Routing.RouteData routeData, System.Collections.Specialized.NameValueCollection requestParams)
        {
            return true;
        }
    }
}
