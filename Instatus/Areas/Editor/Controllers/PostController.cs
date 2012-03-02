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
    public class PostViewModel : BaseViewModel<Post, IDataContext>
    {
        [Category("Overview")]
        [Display(Order = 1)]
        public OverviewViewModel<Post> Overview { get; set; }

        [Category("Overview")]
        [Column("Tags")]
        [Display(Name = "Tags", Order = 2)]
        public MultiSelectList TagsList { get; set; }

        [ScaffoldColumn(false)]
        public int[] Tags { get; set; }

        [Category("Overview")]
        [Column("Organization")]
        [Display(Name = "Organization", Order = 3)]
        public SelectList OrganizationList { get; set; }

        [ScaffoldColumn(false)]
        public int? Organization { get; set; }

        [Category("Video")]
        [Display(Order = 4)]
        public VideoViewModel<Post> Video { get; set; }

        [Category("People")]
        [Display(Order = 5)]
        public PeopleViewModel<Post> People { get; set; }

        [Category("Publishing")]
        [Display(Order = 6)]
        public PublishingViewModel<Post> Publishing { get; set; }

        public override void Load(Post model)
        {
            base.Load(model);

            Tags = model.Tags.IsEmpty() ? null : model.Tags.Select(t => t.Id).ToArray();

            Organization = LoadAssociation<Page, Organization>(model.Parents);
        }

        public override void Save(Post model)
        {            
            base.Save(model);

            model.Tags = SaveMultiAssociation<Tag>(Context.Tags, model.Tags, Tags);
            model.Parents = SaveAssociation<Page, Organization>(Context.Pages, model.Parents, Organization);
        }

        public override void Databind()
        {
            base.Databind();
            
            TagsList = new MultiSelectList(Context.Tags.ToList(), "Id", "Name", Tags);
            OrganizationList = DatabindSelectList<Page, Organization>(Context.Pages, Organization);
        }

        public PostViewModel()
        {
            Overview = new OverviewViewModel<Post>();
            Video = new VideoViewModel<Post>();
            People = new PeopleViewModel<Post>();
            Publishing = new PublishingViewModel<Post>();
        }
    }
    
    [Authorize(Roles = "Editor")]
    [Description("Posts")]
    public class PostController : ScaffoldController<PostViewModel, Post, IDataContext, int>
    {
        public override IEnumerable<Post> Query(IEnumerable<Post> set, WebQuery query)
        {
            return set.ByRecency();
        }
    }
}
