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
    public class FileController : ScaffoldController<FileViewModel, WebLink, FileRepository, int>
    {
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
                LocalStorageBlobService.Save("~/LocalStorage/" + Request.FileInputName(), Request.FileInputStream());
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
    }

    public class FileRepository : IRepository<WebLink>
    {
        public static List<IRule<string>> Rules = new List<IRule<string>>()
        {
            new RegexRule(@"-thumb\.jpg", false),
            new RegexRule(@"-small\.jpg", false),
            new RegexRule(@"-medium\.jpg", false),
            new RegexRule(@"-large\.jpg", false)
        };
        
        public IDbSet<WebLink> Items { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void Dispose()
        {

        }

        public FileRepository()
        {
            var path = HostingEnvironment.MapPath(LocalStorageBlobService.BasePath);
            var files = Directory.GetFiles(path);
            var links = files
                .FilterByRules(Rules)
                .Select(file =>
                {
                    var fileName = Path.GetFileName(file);
                    var uri = LocalStorageBlobService.BasePath + fileName;
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