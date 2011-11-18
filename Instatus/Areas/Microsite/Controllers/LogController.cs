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
using System.ComponentModel.DataAnnotations;

namespace Instatus.Areas.Microsite.Controllers
{   
    [Authorize(Roles = "Administrator")]
    public class LogController : ScaffoldController<BaseViewModel<Log>, Log, BaseDataContext, int>
    {
        public override void ConfigureWebView(WebView<Log> webView)
        {
            webView.Permissions = WebRole.Member.ToPermissions();
            base.ConfigureWebView(webView);
        }
    }
}
