using System;
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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Instatus.Areas.Editor.Controllers
{
    public class TagViewModel : BaseViewModel<Tag>
    {
        [Required]
        public string Name { get; set; }
    }
    
    [Authorize(Roles = "Editor")]
    [Description("Tags")]
    public class TagController : ScaffoldController<TagViewModel, Tag, BaseDataContext, int>
    {

    }
}
