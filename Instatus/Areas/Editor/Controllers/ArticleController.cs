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
using System.ComponentModel.DataAnnotations.Schema;
using Instatus.Entities;

namespace Instatus.Areas.Editor.Controllers
{
    public class ArticleViewModel : PageViewModel
    {
        [Category("Overview")]
        [Display(Order = 1)]
        public OverviewViewModel Overview { get; set; }

        [Category("Overview")]
        [Column("Parent")]
        [Display(Name = "Parent", Order = 2)]
        public SelectList ParentList { get; set; }

        [ScaffoldColumn(false)]
        public int? Parent { get; set; }

        [Category("Body")]
        [Display(Order = 3)]
        public DocumentViewModel Document { get; set; }

        [Category("Video")]
        [Display(Order = 4)]
        public VideoViewModel Video { get; set; }

        [Category("Links")]
        [Display(Order = 5)]
        public CreativeViewModel Creative { get; set; }

        [Category("Meta Tags")]
        [Display(Order = 6)]
        public MetaTagsViewModel MetaTags { get; set; }

        [Category("Custom Markup")]
        [Display(Order = 7)]
        public MarkupViewModel Markup { get; set; }

        [Category("People")]
        [Display(Order = 8)]
        public PeopleViewModel People { get; set; }

        [Category("Publishing")]
        [Display(Order = 9)]
        public PublishingViewModel Publishing { get; set; }

        public override void Load(Page model)
        {
            base.Load(model);

            Parent = ParentId(model, Kind.Article);
        }

        public override void Save(Page model)
        {
            base.Save(model);

            SaveAssociation(model, Kind.Article, Parent);
        }

        public override void Databind()
        {
            base.Databind();

            ParentList = SelectByKind(Kind.Article, Parent);
        }

        public ArticleViewModel()
        {
            Overview = new OverviewViewModel();
            Document = new DocumentViewModel();
            Creative = new CreativeViewModel();
            Video = new VideoViewModel();
            MetaTags = new MetaTagsViewModel();
            Markup = new MarkupViewModel();
            People = new PeopleViewModel();
            Publishing = new PublishingViewModel();
        }
    }

    [Authorize(Roles = "Editor")]
    [Description("Pages")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class ArticleController : ScaffoldController<ArticleViewModel, Page, IApplicationModel, int>
    {
        public override IEnumerable<Page> Query(IEnumerable<Page> set, Query query)
        {
            return set.Where(p => p.Kind == "Article").ByAlphabetical();
        }

        public override Page CreateModelInstance()
        {
            return new Page(Kind.Article);
        }
    }
}
