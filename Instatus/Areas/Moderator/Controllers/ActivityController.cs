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
using Instatus.Entities;

namespace Instatus.Areas.Moderator.Controllers
{
    [Authorize(Roles = "Moderator")]
    public class ActivityController : ScaffoldController<BaseViewModel<Activity>, Activity, IApplicationModel, int>
    {
        public override IEnumerable<Activity> Query(IEnumerable<Activity> set, Query query)
        {
            return Context.Activities
                    .Include(a => a.Page)
                    .Include(a => a.User)
                    .ByRecency();
        }
        
        public override void ConfigureWebView(WebView<Activity> webView)
        {
            base.ConfigureWebView(webView);

            webView.Permissions = new string[] { "Details" };
        }
    }
}
