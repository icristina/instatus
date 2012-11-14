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
    public class BlogPostEditor : EntityMapper<Post, BlogPostEditor>
    {
        private IEntityStorage entityStorage;
        
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

        // Friendly name
        public override string ToString()
        {
            return Title;
        }

        // Mapper
        public override Expression<Func<Post, BlogPostEditor>> GetProjection()
        {
            return p => new BlogPostEditor()
            {
                Id = p.Id,
                Title = p.Name,
                Content = p.Content,
                FriendlyUrl = p.Slug,
                Published = p.Created
            };
        }

        public override Post CreateEntity(BlogPostEditor model)
        {
            return new Post()
            {
                Name = model.Title,
                Content = model.Content,
                Slug = model.FriendlyUrl,
                Category = WellKnown.Kind.BlogPost
            };
        }

        public override BlogPostEditor CreateViewModel(Post entity)
        {
            return GetProjection().Compile()(entity);
        }

        public override void FillEntity(Post entity, BlogPostEditor model)
        {
            entity.Name = model.Title;
            entity.Content = model.Content;
            entity.Slug = model.FriendlyUrl;
        }

        // Constructors
        public BlogPostEditor() { }

        public BlogPostEditor(IEntityStorage entityStorage)
        {
            this.entityStorage = entityStorage;
        }
    }
}