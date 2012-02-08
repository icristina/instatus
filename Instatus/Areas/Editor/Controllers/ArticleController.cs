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
using System.Data.Entity;
using Instatus.Areas.Editor.Models;

namespace Instatus.Areas.Editor.Controllers
{
    public class LinkViewModel : IHasValue
    {        
        public string Name { get; set; }

        [DataType(DataType.Url)]
        public string Uri { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Picture { get; set; }

        [ScaffoldColumn(false)]
        public bool HasValue
        {
	        get {
                return !Uri.IsEmpty() && !Name.IsEmpty();
            }
        }
    }
    
    public class ArticleViewModel : BaseViewModel<Article>
    {
        [Category("Overview")]
        [Display(Order = 1)]
        public OverviewViewModel Overview { get; set; }

        [Category("Body")]
        [Display(Order = 2)]
        public DocumentViewModel Document { get; set; }

        [Category("Links")]
        [ScaffoldColumn(true)]
        [Display(Order = 3)]
        public IEnumerable<LinkViewModel> Links { get; set; }

        [Category("Meta Tags")]
        [Display(Order = 4)]
        public MetaTagsViewModel MetaTags { get; set; }

        [Category("Publishing")]
        [Display(Order = 5)]
        public PublishingViewModel Publishing { get; set; }

        public override void Load(Article model)
        {
            base.Load(model);

            Links = model.Document.Links.Select(l => new LinkViewModel()
            {
                Uri = l.Uri,
                Name = l.Title,
                Picture = l.Picture
            })
            .ToList()
            .Pad(10);
        }

        public override void Save(Article model)
        {
            base.Save(model);

            model.Document.Links = Links.RemoveNullOrEmpty().Select(l => new WebLink()
            {
                Uri = l.Uri,
                Title = l.Name,
                Picture = l.Picture                
            })
            .ToList();
        }

        public ArticleViewModel()
        {
            Overview = new OverviewViewModel();
            Document = new DocumentViewModel();
            MetaTags = new MetaTagsViewModel();
            Publishing = new PublishingViewModel();
        }
    }

    [Authorize(Roles = "Editor")]
    [Description("Articles")]
    public class ArticleController : ScaffoldController<ArticleViewModel, Article, BaseDataContext, int>
    {
        public override IOrderedQueryable<Article> Query(IDbSet<Article> set, WebQuery query)
        {
            return set.OrderBy(o => o.Name);
        }

        public override void ConfigureWebView(WebView<Article> webView)
        {
            webView.Permissions = WebRole.Editor.ToPermissions();
            base.ConfigureWebView(webView);            
        }
    }
}
