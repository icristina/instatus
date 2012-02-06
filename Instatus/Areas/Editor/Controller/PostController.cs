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
    public class PostViewModel : BaseViewModel<Post, BaseDataContext>
    {
        [DisplayName("Friendly Url")]
        [Required]
        [RegularExpression(ValidationPatterns.Slug)]
        public string Slug { get; set; }        
        
        [DisplayName("Title")]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [Column("Tags")]
        [Display(Name = "Tags")]
        public MultiSelectList TagsList { get; set; }

        [ScaffoldColumn(false)]
        public int[] Tags { get; set; }

        [Column("Organization")]
        [Display(Name = "Organization")]
        public SelectList OrganizationList { get; set; }

        [ScaffoldColumn(false)]
        public int Organization { get; set; }

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
