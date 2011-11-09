﻿using System;
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

namespace Instatus.Areas.Microsite.Controllers
{
    public class BlogPostViewModel : BaseViewModel<Post, BaseDataContext>
    {
        [DisplayName("Friendly Url")]
        [Required]
        public string Slug { get; set; }        
        
        [DisplayName("Title")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Body")]
        [DataType(DataType.MultilineText)]
        [Required]
        [AllowHtml]
        public string Description { get; set; }

        [Column("Tags")]
        public MultiSelectList TagsList { get; set; }

        [ScaffoldColumn(false)]
        public int[] Tags { get; set; }

        [Column("Status")]
        public SelectList StatusList { get; set; }

        [ScaffoldColumn(false)]
        public string Status { get; set; }

        public override void Load(Post model)
        {
            Tags = model.Tags.IsEmpty() ? null : model.Tags.Select(t => t.Id).ToArray();
            base.Load(model);
        }

        public override void Save(Post model)
        {
            model.Tags = UpdateList<Tag, int>(Context.Tags, model.Tags, Tags);
            model.User = Context.GetCurrentUser();
            base.Save(model);
        }

        public override void Databind()
        {
            StatusList = new SelectList(new string[] { "Published", "Draft" }, Status);
            TagsList = new MultiSelectList(Context.Tags.ToList(), "Id", "Name", Tags);
        }
    }
    
    [Authorize(Roles = "Editor")]
    public class BlogPostController : ScaffoldController<BlogPostViewModel, Post, BaseDataContext, int>
    {
        public override void ConfigureWebView(WebView<Post> webView)
        {
            webView.Permissions = WebRole.Administrator.ToPermissions();
            base.ConfigureWebView(webView);
        }
    }
}
