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
using Instatus;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Instatus.Areas.Editor.Models;

namespace Instatus.Areas.Editor.Controllers
{
    public class NewsViewModel : BaseViewModel<News>
    {
        [Category("Overview")]
        [Display(Order = 1)]
        public OverviewViewModel<Article> Overview { get; set; }

        [Category("Body")]
        [Display(Order = 2)]
        public DocumentViewModel<Article> Document { get; set; }

        [Category("Publishing")]
        [Display(Order = 3)]
        public PublishingViewModel<Article> Publishing { get; set; }

        public NewsViewModel()
        {
            Overview = new OverviewViewModel<Article>();
            Document = new DocumentViewModel<Article>();
            Publishing = new PublishingViewModel<Article>();
        }
    }
    
    [Authorize(Roles = "Editor")]
    [Description("News")]
    public class NewsController : ScaffoldController<NewsViewModel, News, BaseDataContext, int>
    {
        public override void ConfigureWebView(WebView<News> webView)
        {
            webView.Permissions = WebRole.Editor.ToPermissions();
            base.ConfigureWebView(webView);
        }
    }
}
