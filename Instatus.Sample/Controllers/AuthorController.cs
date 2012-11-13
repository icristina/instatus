using Instatus.Core;
using Instatus.Core.Impl;
using Instatus.Integration.Mvc;
using Instatus.Scaffold.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Sample.Controllers
{
    public class AuthorController : EntityStorageController<Post, PostViewModel> 
    {
        public AuthorController(IEntityStorage entityStorage)
            : base(entityStorage, new EntityMapper<Post, PostViewModel>(
                p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Name,
                    Content = p.Content
                }, 
                p => new Post()
                {
                    Name = p.Title,
                    Content = p.Content
                }, 
                (d, p) =>
                {
                    d.Name = p.Title;
                    d.Content = p.Content;
                }))
        {

        }
    }

    public class PostViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        
        public string Title { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}
