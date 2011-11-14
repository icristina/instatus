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
    public class LinkViewModel : BaseViewModel<Link, BaseDataContext>
    {
        [Required]
        public string Uri { get; set; }

        [Required]
        public string AlternativeUri { get; set; }
    }
    
    [Authorize(Roles = "Administrator")]
    public class LinkController : ScaffoldController<LinkViewModel, Link, BaseDataContext, int>
    {
        public override void ConfigureWebView(WebView<Link> webView)
        {
            webView.Permissions = WebRole.Administrator.ToPermissions();
            base.ConfigureWebView(webView);
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            PubSub.Provider.Publish<ApplicationReset>(new ApplicationReset());
        }
    }
}
