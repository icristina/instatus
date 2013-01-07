using Instatus.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Integration.Mvc
{
    public class FileList : PagedViewModel<string>
    {
        public string VirtualPath { get; set; }

        public string InputId { get; set; }
        public string ThumbnailId { get; set; }

        public bool IsPickList
        {
            get
            {
                return !string.IsNullOrEmpty(InputId);
            }
        }
        
        public FileList(IOrderedQueryable<string> files, int pageIndex, int pageSize)
            : base(files, pageIndex, pageSize)
        {

        }
    }
}