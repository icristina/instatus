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
    [Description("Export")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class ExportController : ScaffoldController<BaseViewModel<Entry>, Entry, IDbSet<Entry>, string>
    {
        private IEnumerable<IDataExport> exports;

        public override IEnumerable<Entry> Query(IEnumerable<Entry> set, Query query)
        {
            return exports.Select(e => new Entry()
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

            ViewData.AddSingle(new Form()
            {
                ActionName = "Download",
                ActionText = "Export",
                HiddenParameters = new List<Parameter>()
                {
                    new Parameter("name", name)
                }
            });

            return View("Edit");
        }

        public override void ConfigureWebView(WebView<Entry> webView)
        {
            base.ConfigureWebView(webView);
            webView.Permissions = new string[] { };
        }

        public override ICollection<IWebCommand> GetCommands(Query query)
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
                UpdateObject(configuration);
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

        public ExportController(IEnumerable<IDataExport> exports)
        {
            this.exports = exports;
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

        public Link GetLink(dynamic viewModel, UrlHelper urlHelper)
        {
            return new Link()
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

        public Link GetLink(dynamic viewModel, UrlHelper urlHelper)
        {
            if (viewModel.Rel == "Configurable")
            {
                return new Link()
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
