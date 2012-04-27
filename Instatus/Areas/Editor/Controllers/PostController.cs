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
using Instatus.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Instatus.Areas.Editor.Controllers
{
    public class PostViewModel : BaseViewModel<Page, IApplicationModel>
    {
        [Category("Overview")]
        [Display(Order = 1)]
        public OverviewViewModel Overview { get; set; }

        //[Category("Overview")]
        //[Column("Tags")]
        //[Display(Name = "Tags", Order = 2)]
        //public MultiSelectList TagsList { get; set; }

        //[ScaffoldColumn(false)]
        //public int[] Tags { get; set; }

        [Category("Overview")]
        [Column("Organization")]
        [Display(Name = "Organization", Order = 3)]
        public SelectList OrganizationList { get; set; }

        [ScaffoldColumn(false)]
        public int? Organization { get; set; }

        [Category("Video")]
        [Display(Order = 4)]
        public VideoViewModel Video { get; set; }

        //[Category("People")]
        //[Display(Order = 5)]
        //public PeopleViewModel<Post> People { get; set; }

        [Category("Publishing")]
        [Display(Order = 6)]
        public PublishingViewModel Publishing { get; set; }

        public override void Load(Page model)
        {
            base.Load(model);

            //Tags = model.Tags.IsEmpty() ? null : model.Tags.Select(t => t.Id).ToArray();

            //Organization = LoadAssociation<Page, Organization>(model.Parents);
        }

        public override void Save(Page model)
        {            
            base.Save(model);

            //model.Tags = SaveMultiAssociation<Tag>(Context.Tags, model.Tags, Tags);
            //model.Parents = SaveAssociation<Page, Organization>(Context.Pages, model.Parents, Organization);
        }

        public override void Databind()
        {
            base.Databind();
            
            //TagsList = new MultiSelectList(Context.Tags.ToList(), "Id", "Name", Tags);
            //OrganizationList = DatabindSelectList<Page, Organization>(Context.Pages, Organization);
        }

        public PostViewModel()
        {
            Overview = new OverviewViewModel();
            Video = new VideoViewModel();
            //People = new PeopleViewModel<Post>();
            Publishing = new PublishingViewModel();
        }
    }
    
    [Authorize(Roles = "Editor")]
    [Description("Posts")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class PostController : ScaffoldController<PostViewModel, Page, IApplicationModel, int>
    {
        public override IEnumerable<Page> Query(IEnumerable<Page> set, Query query)
        {
            return set.Where(p => p.Kind == "Post").ByRecency();
        }

        public override Page CreateModelInstance()
        {           
            return new Page(Kind.Post);
        }
    }
}
