﻿using System;
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
        public override IEnumerable<WebLink> Query(IDbSet<WebLink> set, WebQuery query)
        {
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
                    new SelectCommand()
                };
            }

            return null;
        }

        public override void ConfigureWebView(WebView<WebLink> webView)
        {
            base.ConfigureWebView(webView);
            webView.Permissions = new WebAction[] { WebAction.Create };
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
                .Where(fileName => Rules.All(rule => rule.Evaluate(fileName)))
                .Select(fileName =>
                {
                    var uri = LocalStorageBlobService.BasePath + Path.GetFileName(fileName);
                    return new WebLink()
                    {
                        Uri = uri,
                        Picture = WebMimeType.IsRelativePathPhoto(uri) ? uri : null
                    };
                })
                .ToList();

            Items = new InMemorySet<WebLink>(links);
        }
    }
}