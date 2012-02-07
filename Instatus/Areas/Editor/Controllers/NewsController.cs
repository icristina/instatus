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

namespace Instatus.Areas.Editor.Controllers
{
    public class NewsViewModel : BaseViewModel<News>
    {
        [DisplayName("Friendly Url")]
        [Required]
        [RegularExpression(ValidationPatterns.Slug, ErrorMessage = ValidationMessages.InvalidSlug)]
        public string Slug { get; set; }        
        
        [DisplayName("Title")]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }
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
