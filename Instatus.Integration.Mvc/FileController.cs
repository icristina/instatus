using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Instatus.Core;
using Instatus.Core.Utils;
using System.Web.SessionState;
using System.Net.Http;

namespace Instatus.Integration.Mvc
{
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class FileController : Controller
    {
        public IBlobStorage BlobStorage { get; private set; }

        public ActionResult Index(int take = 250, int skip = 0, string virtualPath = null)
        {
            ViewData.Model = Query(virtualPath)
                .OrderBy(s => s)
                .Skip(skip)
                .Take(take)
                .ToArray();
            
            return View();
        }

        public virtual IQueryable<string> Query(string virtualPath)
        {
            return BlobStorage
                .Query(virtualPath ?? BaseVirtualPath, null)
                .AsQueryable();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewData.Model = new FileModel();

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            var viewModel = new FileModel(AllowedContentTypes, MaximumContentLength);

            TryUpdateModel(viewModel, formCollection);
            
            if (ModelState.IsValid)
            {
                var virtualPath = MapPath(viewModel.FileName);

                using (var outputStream = BlobStorage.OpenWrite(virtualPath, null))
                {
                    viewModel.InputStream.CopyTo(outputStream);
                }

                OnCreated(virtualPath);

                return RedirectToAction("Index");
            }

            return View();
        }

        public virtual string BaseVirtualPath
        {
            get
            {
                return WellKnown.VirtualPath.Media;
            }
        }

        public virtual string[] AllowedContentTypes
        {
            get
            {
                return WellKnown.Whitelist.JpgContentType;
            }
        }

        public virtual int MaximumContentLength
        {
            get
            {
                return 5 * WellKnown.Conversion.BytesPerMegabyte;
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

        public FileController(IBlobStorage blobStorage)
        {
            BlobStorage = blobStorage;
        }
    }
}
