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
    public class ArticleViewModel : BaseViewModel<Article, IDataContext>
    {
        [Category("Overview")]
        [Display(Order = 1)]
        public OverviewViewModel<Article> Overview { get; set; }

        [Category("Overview")]
        [Column("Parent")]
        [Display(Name = "Parent", Order = 2)]
        public SelectList ParentList { get; set; }

        [ScaffoldColumn(false)]
        public int? Parent { get; set; }

        [Category("Body")]
        [Display(Order = 3)]
        public DocumentViewModel<Article> Document { get; set; }

        [Category("Links")]
        [ScaffoldColumn(true)]
        [Display(Order = 4)]
        public IEnumerable<LinkViewModel> Links { get; set; }

        [Category("Meta Tags")]
        [Display(Order = 5)]
        public MetaTagsViewModel<Article> MetaTags { get; set; }

        [Category("Custom Markup")]
        [Display(Order = 6)]
        public MarkupViewModel<Article> Markup { get; set; }

        [Category("People")]
        [Display(Order = 7)]
        public PeopleViewModel<Article> People { get; set; }

        [Category("Publishing")]
        [Display(Order = 8)]
        public PublishingViewModel<Article> Publishing { get; set; }

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

            model.Parents.OfType<Article>().ForFirst(p => Parent = p.Id);
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

            model.Parents.OfType<Article>().ForFirst(p => model.Parents.Remove(p));

            if (Parent.HasValue)
            {
                model.Parents.Add(Context.Pages.Find(Parent));
            }
        }

        public override void Databind()
        {
            base.Databind();

            ParentList = new SelectList(Context.Pages.OfType<Article>(), "Id", "Name", Parent);
        }

        public ArticleViewModel()
        {
            Overview = new OverviewViewModel<Article>();
            Document = new DocumentViewModel<Article>();
            MetaTags = new MetaTagsViewModel<Article>();
            Markup = new MarkupViewModel<Article>();
            People = new PeopleViewModel<Article>();
            Publishing = new PublishingViewModel<Article>();
        }
    }

    [Authorize(Roles = "Editor")]
    [Description("Articles")]
    public class ArticleController : ScaffoldController<ArticleViewModel, Article, IDataContext, int>
    {
        public override IEnumerable<Article> Query(IEnumerable<Article> set, WebQuery query)
        {
            return set.ByAlphabetical();
        }
    }
}
