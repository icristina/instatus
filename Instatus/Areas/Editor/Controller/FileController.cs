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

namespace Instatus.Areas.Editor.Controllers
{
    public class FileViewModel : BaseViewModel<Link>
    {
        public string Name { get; set; }
    }

    [Authorize(Roles = "Editor")]
    [Description("Files")]
    public class FileController : ScaffoldController<FileViewModel, Link, FileRepository, int>
    {
        public override IOrderedQueryable<Link> Query(IDbSet<Link> set, WebQuery query)
        {
            return set.OrderBy(o => o.Name);
        }
    }

    public class FileRepository : IRepository<Link>
    {        
        public IDbSet<Link> Items { get; set; }

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
            var extensions = new string[] { "-small", "-medium" };
            var links = files.Where(f => !extensions.Any(e => f.Contains(e))).Select(f => new Link() {
                Uri = LocalStorageBlobService.BasePath + Path.GetFileName(f)   
            }).ToList();

            Items = new FileSet(links);
        }
    }

    public class FileSet : InMemorySet<Link> {
        public FileSet(List<Link> links) : base(links)
        {

        }
    }
}
