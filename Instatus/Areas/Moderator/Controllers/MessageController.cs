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
using System.Data.Entity;
using System.ComponentModel;

namespace Instatus.Areas.Moderator.Controllers
{
    public class MessageViewModel : BaseViewModel<Note, BaseDataContext>
    {
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Column("PageId")]
        [Display(Name = "Page")]
        [AdditionalMetadata("Required", true)]
        public SelectList PageList { get; set; }

        [ScaffoldColumn(false)]
        public int? PageId { get; set; }

        public override void Databind()
        {
            PageList = new SelectList(Context.Pages.Where(p => p is Application || p is Article).ToList(), "Id", "Name", PageId);
        }
    }
    
    [Authorize(Roles = "Moderator")]
    [Description("Notes")]
    public class MessageController : ScaffoldController<MessageViewModel, Note, BaseDataContext, int>
    {
        public override IEnumerable<Note> Query(IDbSet<Note> set, WebQuery query)
        {
            return set.Where(c => c.Page is Application || c.Page is Article)
                      .OrderBy(c => c.CreatedTime);
        }
    }
}
