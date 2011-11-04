﻿using System;
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

namespace Instatus.Controllers
{
    public class BaseController<TContext> : BaseController where TContext : class
    {
        protected TContext Context;

        protected override void Dispose(bool disposing)
        {
            if (Context is IDisposable)
                ((IDisposable)Context).Dispose();

            base.Dispose(disposing);
        }

        public BaseController()
        {
            Context = DependencyResolver.Current.GetService<TContext>() ?? Activator.CreateInstance<TContext>();
        }
    }
    
    public class BaseController : Controller
    {                
        public HttpApplication Application
        {
            get
            {
                return ControllerContext.HttpContext.ApplicationInstance;
            }
        }

        public ActionResult CommandResult(ICollection<IWebCommand> commands, string commandName, object model)
        {
            var command = commands.First(c => c.Name == commandName);
            command.Execute(model, Url, RouteData, Request.Params);
            return RedirectToIndex();
        }

        public const string ReturnUrlParameter = "returnUrl";

        public ActionResult RedirectToIndex()
        {
            var returnUrl = Request.Params[ReturnUrlParameter];
            
            if (!returnUrl.IsEmpty())
                return Redirect(returnUrl);
            
            return RedirectToAction("Index");
        }

        public ActionResult RedirectToHome()
        {
            return Redirect("/");
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
            
            base.OnActionExecuted(filterContext);
        }
        
        [ChildActionOnly]
        public ActionResult RegisterScripts()
        {           
            using (var db = BaseDataContext.Instance())
            {
                var environment = Application.Setting<string>("Environment");
                var controllerName = RouteData.ControllerName();
                var serviceName = RouteData.AreaName().OrDefault(controllerName);

                ViewData.Model = db.Sources
                    .OfType<Credential>()
                    .FirstOrDefault(s => s.Provider == serviceName && s.Application != null && s.Environment == environment);
            }

            return PartialView();
        }

        [NonAction]
        public ActionResult Article()
        {
            using (var db = BaseDataContext.Instance())
            {
                ViewData.Model = db.GetPage<Article>(RouteData.ActionName());
            }

            return View("Article");
        }

        [NonAction]
        public new ActionResult View()
        {                      
            if (RouteData.WebAction().Equals(WebAction.Details) && ViewData.Model.IsEmpty())
                return HttpNotFound();

            if (Request.IsAjaxRequest())
                return PartialView();

            if (!viewName.IsEmpty())
                return base.View(viewName);

            return base.View();
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
        public ActionResult Content(string content, WebContentType contentType)
        {
            return Content(content, contentType.ToMimeType());
        }

        [NonAction]
        public ActionResult Feed(WebContentType format, SyndicationFeed feed)
        {
            return new FeedResult(format, feed);
        }

        private class FeedResult : ActionResult
        {
            public WebContentType Format { get; set; }
            public SyndicationFeed Feed { get; set; }

            public FeedResult(WebContentType format, SyndicationFeed feed)
            {
                Format = format;
                Feed = feed;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                var response = context.HttpContext.Response;

                SyndicationFeedFormatter formatter;

                switch (Format)
                {
                    case WebContentType.Atom:
                        formatter = Feed.GetAtom10Formatter();
                        break;
                    case WebContentType.Rss:
                        formatter = Feed.GetRss20Formatter();
                        break;
                    default:
                        throw new Exception("Invalid content type");
                }

                response.ContentType = Format.ToMimeType();

                using (var xmlWriter = new XmlTextWriter(response.Output))
                {
                    xmlWriter.Formatting = Formatting.Indented;
                    formatter.WriteTo(xmlWriter);
                }
            }
        }
    }
}
