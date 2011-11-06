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
    public class TagViewModel : BaseViewModel<Tag>
    {
        [Required]
        public string Name { get; set; }
    }
    
    [Authorize(Roles = "Administrator")]
    public class TagController : ScaffoldController<TagViewModel, Tag, BaseDataContext, int>
    {
        public override void ConfigureWebView(WebView<Tag> webView)
        {
            webView.Permissions = WebRole.Administrator.ToPermissions();
            base.ConfigureWebView(webView);
        }
    }
}
