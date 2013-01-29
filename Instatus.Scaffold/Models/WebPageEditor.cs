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
    public class WebPageEditor
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Slug { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType("Picture")]
        public string Picture { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        [Required]
        public string Content { get; set; }
        
        public override string ToString()
        {
            return string.Format("{0} ({1})", Title, Slug);
        }

        // mapper
        public class Mapper : IMapper<Post, WebPageEditor>
        {
            public Expression<Func<Post, WebPageEditor>> GetProjection()
            {
                return p => new WebPageEditor()
                {
                    Id = p.Id,
                    Slug = p.Slug,
                    Title = p.Name,
                    Picture = p.Picture,
                    Content = p.Content
                };
            }

            public Post CreateEntity(WebPageEditor model)
            {
                return new Post()
                {
                    Name = model.Title,
                    Picture = model.Picture,
                    Content = model.Content
                };
            }

            public WebPageEditor CreateViewModel(Post entity)
            {
                return GetProjection().Compile()(entity);
            }

            public void FillEntity(Post entity, WebPageEditor model)
            {
                entity.Name = model.Title;
                entity.Picture = model.Picture;
                entity.Content = model.Content;
            }
        }
    }
}