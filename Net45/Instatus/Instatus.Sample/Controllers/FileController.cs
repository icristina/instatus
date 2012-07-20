using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Core;
using Instatus.Integration.Mvc;

namespace Instatus.Sample.Controllers
{
    public class FileController : FileStorageController
    {
        public FileController(IBlobStorage blobStorage)
            : base(blobStorage)
        {

        }
    }
}
