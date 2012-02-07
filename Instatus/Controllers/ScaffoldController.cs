using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Instatus.Models;
using System.Data;
using Instatus.Web;
using System.Linq.Expressions;
using Instatus.Data;
using System.ComponentModel;

namespace Instatus.Controllers
{
    public class ScaffoldController<TViewModel, TModel, TContext, TKey> : BaseController<TContext> 
        where TViewModel : IViewModel<TModel>
        where TContext : class 
        where TModel : class
    {       
        private IDbSet<TModel> set;

        public ScaffoldController()
        {
            CreateSet();
        }

        public ActionResult Index(WebQuery query)
        {
            var webView = new WebView<TModel>(Query(set, query), query);
            ConfigureWebView(webView);
            return View("~/Views/Shared/Index.cshtml", webView); // view name hard coded in case a view in parent project with the same name
        }

        public virtual IOrderedQueryable<TModel> Query(IDbSet<TModel> set, WebQuery query)
        {
            return set.OrderBy(m => true);
        }

        public virtual void ConfigureWebView(WebView<TModel> webView) {
            webView.Navigation = Url.Controllers();
            webView.Commands = GetCommands();
            webView.Document = new WebDocument()
            {
                Title = ControllerContext.Controller.GetCustomAttributeValue<DescriptionAttribute, string>(d => d.Description)
            };
        }

        public virtual void AttachContext(TViewModel viewModel)
        {
            if (viewModel is BaseViewModel<TModel, TContext>)
                (viewModel as BaseViewModel<TModel, TContext>).Context = Context;
        }

        [HttpGet]
        public ActionResult Details(TKey id)
        {
            ViewData.Model = set.Find(id);
            return View("~/Views/Shared/Details.cshtml");
        }

        [HttpGet]
        public ActionResult Edit(TKey id)
        {
            var model = set.Find(id);
            var viewModel = Activator.CreateInstance<TViewModel>();
            AttachContext(viewModel);
            viewModel.Load(model);
            viewModel.Databind();
            return View("~/Views/Shared/Edit.cshtml", viewModel);
        }

        [HttpPost]        
        public ActionResult Edit(TKey id, TViewModel viewModel)
        {
            AttachContext(viewModel);         
            
            if (ModelState.IsValid)
            {
                var model = set.Find(id);
                viewModel.Save(model);
                SaveChanges();

                return RedirectToAction("Details", new { id = id });
            }

            viewModel.Databind();

            return View("~/Views/Shared/Edit.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = Activator.CreateInstance<TModel>();
            var viewModel = Activator.CreateInstance<TViewModel>();

            AttachContext(viewModel);
            viewModel.Load(model);
            viewModel.Databind();

            return View("~/Views/Shared/Create.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult Create(TViewModel viewModel)
        {
            AttachContext(viewModel);           
            
            if (ModelState.IsValid)
            {
                var model = Activator.CreateInstance<TModel>();
                viewModel.Save(model);
                set.Add(model);
                SaveChanges();


                return RedirectToAction("Details", new { id = model.GetKey() });
            }

            viewModel.Databind();

            return View("~/Views/Shared/Create.cshtml", viewModel);
        }

        public virtual ICollection<IWebCommand> GetCommands()
        {
            return new List<IWebCommand>();
        }

        public ActionResult Command(TKey id, string commandName)
        {
            return CommandResult(GetCommands(), commandName, set.Find(id));
        }

        [HttpPost]
        public ActionResult Delete(TKey id)
        {
            var model = set.Find(id);
            set.Remove(model);
            SaveChanges();
            return RedirectToIndex();
        }

        public virtual void CreateSet()
        {
            if (Context is DbContext)
            {
                set = (Context as DbContext).Set<TModel>();
            }
            else if (Context is IDbSet<TModel>)
            {
                set = Context as IDbSet<TModel>;
            }
            else if (Context is IRepository<TModel>)
            {
                set = (Context as IRepository<TModel>).Items;
            }
            else
            {
                throw new Exception("Unsupported context");
            }
        }

        public virtual void SaveChanges()
        {
            if (Context is DbContext)
            {
                (Context as DbContext).SaveChanges();
            }
            else if (Context is IRepository<TModel>)
            {
                (Context as IRepository<TModel>).SaveChanges();
            }
        }
    }
}
