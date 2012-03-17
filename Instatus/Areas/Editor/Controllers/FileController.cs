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
    [AdditionalMetadata("File", true)]
    public class FileViewModel : BaseViewModel<WebLink>
    {
        [UIHint("File")]
        public string File { get; set; }
    }

    [Authorize(Roles = "Editor")]
    [Description("Files")]
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class FileController : ScaffoldController<FileViewModel, WebLink, FileRepository, int>
    {
        private IBlobService blobService;
        
        public override IEnumerable<WebLink> Query(IEnumerable<WebLink> set, WebQuery query)
        {
            if (query.Mode == WebMode.Index)
            {
                if (query.Filter.IsEmpty())
                    query.Filter = "A";
                
                if (query.Filter.Match(WebQuery.NonWordCharacter))
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

        public override ICollection<IWebCommand> GetCommands(WebQuery query)
        {
            if (query.Command.Match("picker"))
            {
                return new List<IWebCommand>() {
                    new GenericCommand("select")
                };
            }

            return null;
        }

        public override void ConfigureWebView(WebView<WebLink> webView)
        {
            base.ConfigureWebView(webView);

            webView.Permissions = new WebAction[] { WebAction.Create };

            webView.Mode = WebUtility.CreateSelectList(
                new WebMode[] { WebMode.PagedList, WebMode.Index }, 
                webView.Query.Mode,
                new string[] { "List", "Alphabetical" });
        }

        [ImportingConstructor]
        public FileController(IBlobService blobService)
        {
            this.blobService = blobService;
            Context = new FileRepository(blobService);
        }
    }

    public class FileRepository : IRepository<WebLink>
    {
        public IDbSet<WebLink> Items { get; set; }

        public void SaveChanges()
        {

        }

        public FileRepository(IBlobService blobService)
        {
            var files = blobService.Query(null);
            var links = files
                .Select(file =>
                {
                    var fileName = Path.GetFileName(file);
                    var uri = FileSystemBlobService.BasePath + fileName;
                    return new WebLink()
                    {
                        Uri = uri,
                        Picture = WebMimeType.IsRelativePathPhoto(uri) ? uri : null,
                        Title = fileName
                    };
                })
                .ToList();

            Items = new FileMemorySet(links);
        }
    }

    internal class FileMemorySet : InMemorySet<WebLink> {
        public FileMemorySet(IEnumerable<WebLink> links)
            : base(links)
        {

        }
    }
}