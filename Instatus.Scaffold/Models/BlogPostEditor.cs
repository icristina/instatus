using Instatus.Core;
using Instatus.Core.Extensions;
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

        [DataType(DataType.MultilineText)]
        [Required]
        public string Abstract { get; set; }

        [DataType("Picture")]
        public string Picture { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        [Required]
        public string Content { get; set; }

        public string Tags { get; set; }

        public override string ToString()
        {
            return Title;
        }

        // mapper
        public class Mapper : IMapper<Post, BlogPostEditor>
        {
            private IEntityStorage entityStorage;

            public Expression<Func<Post, BlogPostEditor>> GetProjection()
            {
                return p => new BlogPostEditor()
                {
                    Id = p.Id,
                    Title = p.Name,
                    Abstract = p.Description,
                    Content = p.Content,
                    FriendlyUrl = p.Slug,
                    Published = p.Created,
                    Picture = p.Picture
                };
            }

            public Post CreateEntity(BlogPostEditor model)
            {
                var post = new Post()
                {
                    Name = model.Title,
                    Description = model.Abstract,
                    Content = model.Content,
                    Slug = model.FriendlyUrl,
                    Category = WellKnown.Kind.BlogPost,
                    Picture = model.Picture
                };

                SyncTags(post, model.Tags);

                return post;
            }

            public BlogPostEditor CreateViewModel(Post entity)
            {
                var blogPostEditor = GetProjection().Compile()(entity);

                blogPostEditor.Tags = string.Join(", ", entity.Tags.Select(t => t.Name).ToArray());

                return blogPostEditor;
            }

            public void FillEntity(Post entity, BlogPostEditor model)
            {
                entity.Name = model.Title;
                entity.Description = model.Abstract;
                entity.Content = model.Content;
                entity.Slug = model.FriendlyUrl;
                entity.Picture = model.Picture;

                SyncTags(entity, model.Tags);
            }

            private void SyncTags(Post post, string tags)
            {
                post.Tags.Clear();

                if (!string.IsNullOrWhiteSpace(tags))
                {
                    var tagNames = tags.AsDistinctArray();

                    Taxonomy taxonomy = null;

                    foreach (var tag in tagNames)
                    {
                        var existingTag = entityStorage.Set<Tag>().FirstOrDefault(t => t.Name == tag);

                        if (existingTag != null)
                        {
                            post.Tags.Add(existingTag);
                        }
                        else
                        {
                            post.Tags.Add(new Tag()
                            {
                                Name = tag,
                                Taxonomy = taxonomy ?? (taxonomy = entityStorage.Set<Taxonomy>().FirstOrDefault())
                            });
                        }
                    }
                }
            }          
  
            public Mapper(IEntityStorage entityStorage)
            {
                this.entityStorage = entityStorage;
            }
        }
    }
}