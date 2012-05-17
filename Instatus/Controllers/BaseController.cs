using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.ServiceModel.Syndication;
using Instatus.Data;
using Instatus.Models;
using Instatus.Web;
using System.Net;
using System.Configuration;
using System.Web.Helpers;
using System.Web.SessionState;

namespace Instatus.Controllers
{
    public class BaseController<TContext> : BaseController where TContext : class
    {
        private TContext context;

        public TContext Context
        {
            get
            {
                if (context == null)
                {
                    context = DependencyResolver.Current.GetService<TContext>();
                }

                return context;
            }
            set
            {
                context = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.TryDispose();
            }

            base.Dispose(disposing);
        }
    }

    [SessionState(SessionStateBehavior.Disabled)]
    public class BaseController : Controller
    {
        public void UpdateObject(object viewModel)
        {
            var modelBindingContext = new ModelBindingContext()
            {
                ValueProvider = ValueProvider,
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => viewModel, viewModel.GetType()),
                ModelState = new ModelStateDictionary()
            };

            ModelBinders.Binders.DefaultBinder.BindModel(ControllerContext, modelBindingContext);
        }        

        public ActionResult CommandResult(ICollection<IWebCommand> commands, string commandName, object model)
        {
            var command = commands.First(c => c.Name == commandName);
            command.Execute(model, Url, RouteData, Request.Params);
            return RedirectToIndex();
        }

        public ActionResult RedirectToIndex()
        {
            return RedirectToAction("Index", null);
        }

        public ActionResult RedirectToController(string controllerName)
        {
            return RedirectToAction("Index", controllerName, null);
        }

        public new ActionResult RedirectToAction(string actionName, object values = null)
        {
            var returnUrl = Request.Unvalidated(WebConstant.QueryParameter.ReturnUrl); // fix to allow redirect to action from form that includes HTML
            
            if (!returnUrl.IsEmpty())
                return Redirect(returnUrl);
            
            return base.RedirectToAction(actionName, values);
        }

        public ActionResult RedirectToHome()
        {
            return Redirect(WebPath.Home);
        }

        public ActionResult ErrorResult()
        {
            return new HttpStatusCodeResult((int)HttpStatusCode.InternalServerError);
        }

        public ActionResult SuccessResult()
        {
            return new HttpStatusCodeResult((int)HttpStatusCode.OK);
        } 
        
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.IsChildAction && !Response.IsRequestBeingRedirected)
            {
                Response.AddHeader("p3p", "CP=\"CAO PSA OUR\""); // cookies in iframes for IE
            }

            if (Request.IsAjaxRequest() && ConfigurationManager.AppSettings.Value<string>(WebConstant.AppSetting.Environment).AsEnum<Deployment>() == Deployment.Development)
            {
                System.Threading.Thread.Sleep(3000);
            }

            base.OnActionExecuted(filterContext);
        }       

        [NonAction]
        public new ActionResult View()
        {                      
            if (RouteData.ActionName().Match("Details") && ViewData.Model.IsEmpty())
                return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return viewName.IsEmpty() ? base.PartialView() : base.PartialView(viewName);
            }

            return viewName.IsEmpty() ? base.View() : base.View(viewName);
        }

        [NonAction]
        public ActionResult Error(object viewModel = null, string errorMessage = null)
        {
            ModelState.AddGenericError(errorMessage ?? WebPhrase.ErrorDescription);
            return View(viewModel);
        }

        [NonAction]
        public ActionResult PartialViewOrEmpty()
        {
            if (ViewData.Model.IsEmpty())
                return new EmptyResult();

            return PartialView();
        }

        private string viewName;

        [NonAction]
        public new ActionResult View(string viewName)
        {
            this.viewName = viewName;
            return View();
        }

        [NonAction]
        public new ActionResult View(object viewModel)
        {
            ViewData.Model = viewModel;
            return View();
        }

        [NonAction]
        public new ActionResult View(string viewName, object viewModel)
        {
            ViewData.Model = viewModel;
            this.viewName = viewName;
            return View();
        }

        [NonAction]
        public ActionResult Content(string content, WebContentType contentType)
        {
            return Content(content, contentType.ToMimeType());
        }

        [NonAction]
        public ActionResult Feed(WebContentType format, SyndicationFeed feed)
        {
            return new SyndicationFeedResult(format, feed);
        }
    }
}
