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
    public class PostViewModel : BaseViewModel<Post, BaseDataContext>
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

        [Category("Publishing")]
        [Display(Order = 5)]
        public PublishingViewModel<Post> Publishing { get; set; }

        public override void Load(Post model)
        {
            base.Load(model);

            Tags = model.Tags.IsEmpty() ? null : model.Tags.Select(t => t.Id).ToArray();
            model.Parents.OfType<Organization>().ForFirst(o => Organization = o.Id);
        }

        public override void Save(Post model)
        {            
            base.Save(model);

            model.Tags = UpdateList<Tag, int>(Context.Tags, model.Tags, Tags);

            if (Organization.HasValue)
            {
                model.Parents.OfType<Organization>().ForFirst(o => model.Parents.Remove(o));
                model.Parents.Add(Context.Pages.Find(Organization));
            }
        }

        public override void Databind()
        {
            base.Databind();
            
            TagsList = new MultiSelectList(Context.Tags.ToList(), "Id", "Name", Tags);
            OrganizationList = new SelectList(Context.Pages.OfType<Organization>(), "Id", "Name", Organization);
        }

        public PostViewModel()
        {
            Overview = new OverviewViewModel<Post>();
            Video = new VideoViewModel<Post>();
            Publishing = new PublishingViewModel<Post>();
        }
    }
    
    [Authorize(Roles = "Editor")]
    [Description("Posts")]
    public class PostController : ScaffoldController<PostViewModel, Post, BaseDataContext, int>
    {
        public override IEnumerable<Post> Query(IEnumerable<Post> set, WebQuery query)
        {
            return set.ByRecency();
        }
    }
}
