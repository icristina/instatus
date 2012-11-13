using Instatus.Core;
using Instatus.Core.Impl;
using Instatus.Core.Models;
using Instatus.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Sample.Controllers
{
    public class AdminController : KeyValueStorageController<Document, PostViewModel2>
    {
        public AdminController(IKeyValueStorage<Document> keyValueStorage)
            : base(keyValueStorage, new EntityMapper<Document, PostViewModel2>(d => new PostViewModel2()
            {
                Title = d.Title,
                Description = d.Description
            }, p => new Document()
            {
                Title = p.Title,
                Description = p.Description
            }, (d, p) =>
            {
                d.Title = p.Title;
                d.Description = p.Description;
            }))
        {

        }
    }

    public class PostViewModel2
    {
        [HiddenInput(DisplayValue = false)]
        public string Key { get; set; }
        
        public string Title { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
