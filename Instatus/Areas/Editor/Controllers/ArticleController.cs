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
    public class ArticleViewModel : BaseViewModel<Article, IApplicationModel>
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

        [Category("Video")]
        [Display(Order = 4)]
        public VideoViewModel<Article> Video { get; set; }

        [Category("Links")]
        [Display(Order = 5)]
        public CreativeViewModel<Article> Creative { get; set; }

        [Category("Meta Tags")]
        [Display(Order = 6)]
        public MetaTagsViewModel<Article> MetaTags { get; set; }

        [Category("Custom Markup")]
        [Display(Order = 7)]
        public MarkupViewModel<Article> Markup { get; set; }

        [Category("People")]
        [Display(Order = 8)]
        public PeopleViewModel<Article> People { get; set; }

        [Category("Publishing")]
        [Display(Order = 9)]
        public PublishingViewModel<Article> Publishing { get; set; }

        public override void Load(Article model)
        {
            base.Load(model);

            Parent = LoadAssociation<Page, Article>(model.Parents);
        }

        public override void Save(Article model)
        {
            base.Save(model);

            model.Parents = SaveAssociation<Page, Article>(Context.Pages, model.Parents, Parent);
        }

        public override void Databind()
        {
            base.Databind();

            ParentList = DatabindSelectList<Page, Article>(Context.Pages, Parent);
        }

        public ArticleViewModel()
        {
            Overview = new OverviewViewModel<Article>();
            Document = new DocumentViewModel<Article>();
            Creative = new CreativeViewModel<Article>();
            Video = new VideoViewModel<Article>();
            MetaTags = new MetaTagsViewModel<Article>();
            Markup = new MarkupViewModel<Article>();
            People = new PeopleViewModel<Article>();
            Publishing = new PublishingViewModel<Article>();
        }
    }

    [Authorize(Roles = "Editor")]
    [Description("Articles")]
    public class ArticleController : ScaffoldController<ArticleViewModel, Article, IApplicationModel, int>
    {
        public override IEnumerable<Article> Query(IEnumerable<Article> set, WebQuery query)
        {
            return set.ByAlphabetical();
        }
    }
}
