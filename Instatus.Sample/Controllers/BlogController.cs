using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Scaffold.Entities;
using Instatus.Integration.Mvc;
using Instatus.Core.Extensions;

namespace Instatus.Sample.Controllers
{
    public class BlogController : Instatus.Scaffold.Controllers.BlogController
    {
        public BlogController(IEntityStorage entityStorage) 
            : base(entityStorage)
        {

        }
    }
}