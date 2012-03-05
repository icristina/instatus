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
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel;

namespace Instatus.Areas.Developer.Controllers
{   
    [Authorize(Roles = "Developer")]
    [Description("Logs")]
    public class LogController : ScaffoldController<BaseViewModel<Log>, Log, IApplicationContext, int>
    {
        public override IEnumerable<Log> Query(IEnumerable<Log> set, WebQuery query)
        {
            return set.OrderByDescending(d => d.CreatedTime);
        }

        public override void ConfigureWebView(WebView<Log> webView)
        {
            base.ConfigureWebView(webView);

            webView.Permissions = WebRole.Member.ToPermissions();            
        }
    }
}
