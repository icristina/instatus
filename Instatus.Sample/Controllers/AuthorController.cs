using Instatus.Core;
using Instatus.Core.Impl;
using Instatus.Integration.Mvc;
using Instatus.Scaffold.Entities;
using Instatus.Scaffold.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Sample.Controllers
{
    public class AuthorController : EntityStorageController<Post, BlogPostEditor> 
    {
        public override IOrderedQueryable<BlogPostEditor> Query(string orderBy, string filter)
        {
            return EntitySet
                .Select(Mapper.Projection<Post, BlogPostEditor>())
                .OrderByDescending(b => b.Published);
        }
        
        public AuthorController(IEntityStorage entityStorage, BlogPostEditor mapper)
            : base(entityStorage, mapper)
        {

        }
    }
}
