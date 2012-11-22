using Instatus.Integration.Mvc;
using Instatus.Scaffold.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Models
{
    public class Blog : PagedViewModel<BlogPost>
    {
        public TagCloud TagCloud { get; set; }
        
        public Blog(IOrderedQueryable<BlogPost> posts, int pageIndex, int pageSize)
            : base(posts, pageIndex, pageSize)
        {

        }

        public Blog(IOrderedQueryable<BlogPost> posts, int pageIndex, int firstPageSize, int defaultPageSize)
            : base(posts, pageIndex, firstPageSize, defaultPageSize)
        {

        }
    }
}