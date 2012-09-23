using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Instatus.Core;
using Instatus.Core.Utils;
using System.Web.SessionState;

namespace Instatus.Integration.Mvc
{
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class FileStorageController : Controller
    {
        public IBlobStorage BlobStorage { get; private set; }

        public ActionResult Index(int take = 50, int skip = 0, string virtualPath = null)
        {
            ViewData.Model = BlobStorage
                .Query(virtualPath ?? BaseVirtualPath)
                .OrderBy(s => s)
                .Skip(skip)
                .Take(take)
                .Select(f => BlobStorage.MapPath(f))
                .ToArray();
            
            return View();
        }

        public virtual IQueryable<string> Query(string virtualPath)
        {
            return BlobStorage.Query(virtualPath).AsQueryable();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewData.Model = new FileStorageViewModel();

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            var viewModel = new FileStorageViewModel(AllowedMimeTypes, MaximumContentLength);

            TryUpdateModel(viewModel, formCollection);
            
            if (ModelState.IsValid)
            {
                var virtualPath = MapPath(viewModel.FileName);
                
                BlobStorage.Upload(virtualPath, viewModel.InputStream, null);

                OnCreated(virtualPath);

                return RedirectToAction("Index");
            }

            return View();
        }

        public virtual string BaseVirtualPath
        {
            get
            {
                return "~/Media";
            }
        }

        public virtual string[] AllowedMimeTypes
        {
            get
            {
                return new string[] { "image/jpeg", "image/pjpeg" };
            }
        }

        public virtual int MaximumContentLength
        {
            get
            {
                return 5 * 1048576; // 5Mb
            }
        }

        public virtual string MapPath(string fileName)
        {
            return BaseVirtualPath.TrimEnd(PathBuilder.DelimiterChars) 
                + "/" 
                + Path.GetFileNameWithoutExtension(fileName) 
                + ".jpg";
        }

        public virtual void OnCreated(string virtualPath)
        {

        }

        public FileStorageController(IBlobStorage blobStorage)
        {
            BlobStorage = blobStorage;
        }
    }
}
