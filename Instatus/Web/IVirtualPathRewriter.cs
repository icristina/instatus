using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public interface IVirtualPathRewriter
    {
        string RewriteVirtualPath(string virtualPath);
    }
}