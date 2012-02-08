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
        public OverviewViewModel Overview { get; set; }

        [Category("Overview")]
        [Column("Tags")]
        [Display(Name = "Tags")]
        public MultiSelectList TagsList { get; set; }

        [ScaffoldColumn(false)]
        public int[] Tags { get; set; }

        [Category("Overview")]
        [Column("Organization")]
        [Display(Name = "Organization")]
        public SelectList OrganizationList { get; set; }

        [ScaffoldColumn(false)]
        public int Organization { get; set; }

        [Category("Publishing")]
        [Display(Order = 5)]
        public PublishingViewModel Publishing { get; set; }

        public override void Load(Post model)
        {
            Tags = model.Tags.IsEmpty() ? null : model.Tags.Select(t => t.Id).ToArray();
            base.Load(model);
        }

        public override void Save(Post model)
        {
            model.Tags = UpdateList<Tag, int>(Context.Tags, model.Tags, Tags);
            base.Save(model);
        }

        public override void Databind()
        {            
            TagsList = new MultiSelectList(Context.Tags.ToList(), "Id", "Name", Tags);
            OrganizationList = new SelectList(Context.Pages.OfType<Organization>(), "Id", "Name", Organization);
        }

        public PostViewModel()
        {
            Overview = new OverviewViewModel();
            Publishing = new PublishingViewModel();
        }
    }
    
    [Authorize(Roles = "Editor")]
    [Description("Posts")]
    public class PostController : ScaffoldController<PostViewModel, Post, BaseDataContext, int>
    {
        public override void ConfigureWebView(WebView<Post> webView)
        {
            webView.Permissions = WebRole.Administrator.ToPermissions();
            base.ConfigureWebView(webView);
        }
    }
}
