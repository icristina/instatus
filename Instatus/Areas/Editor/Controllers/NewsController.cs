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
using Instatus;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Instatus.Areas.Editor.Models;
using Instatus.Entities;

namespace Instatus.Areas.Editor.Controllers
{
    public class NewsViewModel : BaseViewModel<Page, IApplicationModel>
    {
        [Category("Overview")]
        [Display(Order = 1)]
        public OverviewViewModel Overview { get; set; }

        [Category("Body")]
        [Display(Order = 2)]
        public DocumentViewModel Document { get; set; }

        [Category("Call To Action")]
        [Display(Order = 3)]
        public CallToActionViewModel CallToAction { get; set; }

        [Category("Publishing")]
        [Display(Order = 4)]
        public PublishingViewModel Publishing { get; set; }

        public NewsViewModel()
        {
            Overview = new OverviewViewModel();
            Document = new DocumentViewModel();
            CallToAction = new CallToActionViewModel();
            Publishing = new PublishingViewModel();
        }
    }
    
    [Authorize(Roles = "Editor")]
    [Description("News")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class NewsController : ScaffoldController<NewsViewModel, Page, IApplicationModel, int>
    {
        public override IEnumerable<Page> Query(IEnumerable<Page> set, Query query)
        {
            return set.Where(p => p.Kind == "News").ByRecency();
        }

        public override Page CreateModelInstance()
        {
            return new Page(Kind.News);
        }
    }
}
