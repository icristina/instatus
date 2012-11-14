using Instatus.Core;
using Instatus.Core.Impl;
using Instatus.Scaffold.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Scaffold.Models
{
    public class BlogPostEditor
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime Published { get; set; }

        [Required]
        [Display(Name = "Friendly Url")]
        [RegularExpression(WellKnown.RegularExpression.Slug)]
        public string FriendlyUrl { get; set; }

        [Required]
        public string Title { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        [Required]
        public string Content { get; set; }

        public string Tags { get; set; }

        public override string ToString()
        {
            return Title;
        }

        public class Mapper : EntityMapper<Post, BlogPostEditor>
        {
            private IEntityStorage entityStorage;
            
            public Mapper(IEntityStorage entityStorage)
            {
                this.entityStorage = entityStorage;
                
                ProjectEntityToViewModel = p => new BlogPostEditor()
                {
                    Id = p.Id,
                    Title = p.Name,
                    Content = p.Content,
                    FriendlyUrl = p.Slug,
                    Published = p.Created
                };

                MapEntityToViewModel = ProjectEntityToViewModel.Compile(); 

                MapViewModelToEntity = p => new Post()
                {
                    Name = p.Title,
                    Content = p.Content,
                    Slug = p.FriendlyUrl,
                    Category = WellKnown.Kind.BlogPost
                };

                InjectViewModelValuesToEntity = (d, p) =>
                {
                    d.Name = p.Title;
                    d.Content = p.Content;
                    d.Slug = p.FriendlyUrl;
                };
            }
        }
    }
}