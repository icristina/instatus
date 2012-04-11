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
using System.Data.Entity;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace Instatus.Areas.Developer.Controllers
{   
    [Authorize(Roles = "Developer")]
    [Description("Logs")]
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class LogController : ScaffoldController<BaseViewModel<ITimestamp>, ITimestamp, ILoggingService, int>
    {
        public override IEnumerable<ITimestamp> Query(IEnumerable<ITimestamp> set, WebQuery query)
        {
            return Context.Query();
        }

        public override void ConfigureWebView(WebView<ITimestamp> webView)
        {
            base.ConfigureWebView(webView);

            webView.Permissions = null;
        }

        public ActionResult ThrowException(string message)
        {
            throw new Exception(message);
        }
    }
}
