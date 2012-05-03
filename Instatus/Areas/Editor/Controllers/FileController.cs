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
using Instatus;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web.Hosting;
using System.ComponentModel.Composition;
using Instatus.Commands;
using System.Text.RegularExpressions;

namespace Instatus.Areas.Editor.Controllers
{
    [AllowUpload]
    public class FileViewModel : BaseViewModel<Link>
    {
        [DataType(WebConstant.DataType.File)]
        public string File { get; set; }
    }

    [Authorize(Roles = WebConstant.Role.Editor)]
    [Description("Files")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class FileController : ScaffoldController<FileViewModel, Link, IDbSet<Link>, int>
    {
        private IBlobService blobService;
        
        public override IEnumerable<Link> Query(IEnumerable<Link> set, Query query)
        {
            var files = blobService.Query();

            set = files
                .Select(file =>
                {
                    return new Link()
                    {
                        Uri = file,
                        Picture = WebMimeType.IsRelativePathPhoto(file) ? file : null,
                        Title = Path.GetFileName(file)
                    };
                })
                .ToList();            
            
            if (query.ViewMode == ViewMode.Index)
            {
                if (query.Filter.IsEmpty())
                    query.Filter = "A";
                
                if (query.Filter.Match("0"))
                {
                    set = set.Where(l => !Generator.UpperCaseLetters.Any(c => l.Title.ToUpper().StartsWith(c.ToString())));
                }
                else
                {
                    set = set.Where(l => l.Title.ToUpper().StartsWith(query.Filter));
                }
            }
            
            return set.OrderBy(l => l.Uri);
        }

        [HttpPost]
        public override ActionResult Create(FileViewModel viewModel)
        {
            if (Request.HasFile())
            {
                blobService.Save(null, Request.FileInputName(), Request.FileInputStream());
            }

            return RedirectToIndex();
        }

        public override ICollection<IWebCommand> GetCommands(Query query)
        {
            if (query.Command.Match("picker"))
            {
                return new List<IWebCommand>() {
                    new GenericCommand("select")
                };
            }

            return null;
        }

        public override void ConfigureWebView(WebView<Link> webView)
        {
            base.ConfigureWebView(webView);

            webView.Permissions = new string[] { "Create" };

            webView.Mode = WebUtility.CreateSelectList(
                new ViewMode[] { ViewMode.PagedList, ViewMode.Index }, 
                webView.Query.ViewMode,
                new string[] { "List", "Alphabetical" });
        }

        [Authorize(Roles = "Developer")]
        public ActionResult RegenerateThumbnails()
        {
            var images = blobService.Query().Where(f => WebMimeType.IsRelativePathPhoto(f));

            foreach (var image in images)
            {
                blobService.GenerateSize(image, ImageSize.Thumb, null, true);
            }

            return Content("Complete");
        }

        [ImportingConstructor]
        public FileController(IBlobService blobService)
        {
            this.blobService = blobService;
        }
    }
}