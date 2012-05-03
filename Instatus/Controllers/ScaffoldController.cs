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
        public static string IndexViewName = "~/Views/Shared/Index.cshtml"; // view name hard coded in case a view in parent project with the same name
        public static string DetailsViewName = "~/Views/Shared/Details.cshtml";
        public static string EditViewName = "~/Views/Shared/Edit.cshtml";
        public static string CreateViewName = "~/Views/Shared/Edit.cshtml";        

        private IDbSet<TModel> set;
        
        public IDbSet<TModel> Set
        {
            get
            {
                if (set == null)
                {
                    if (Context is DbContext)
                    {
                        set = (Context as DbContext).Set<TModel>();
                    }
                    else if (Context is IDbContext)
                    {
                        set = (Context as IDbContext).Set<TModel>();
                    }
                    else if (Context is IDbSet<TModel>)
                    {
                        set = Context as IDbSet<TModel>;
                    }
                    else if (Context is IRepository<TModel>)
                    {
                        set = (Context as IRepository<TModel>).Items;
                    }
                }

                return set;
            }
        }

        public ActionResult Index(Query query)
        {
            var webView = new WebView<TModel>(Query(Set, query), query);
            ConfigureWebView(webView);
            return View(IndexViewName, webView);
        }

        public virtual IEnumerable<TModel> Query(IEnumerable<TModel> set, Query query)
        {
            return set.OrderBy(m => true);
        }

        public virtual void ConfigureWebView(WebView<TModel> webView) {
            var controller = ControllerContext.Controller;
            
            webView.Commands = GetCommands(webView.Query);
            webView.Document = new Document()
            {
                Title = controller.GetCustomAttributeValue<DescriptionAttribute, string>(d => d.Description)
            };

            webView.Permissions = new string[] { "Index", "Details", "Edit", "Create", "Delete" };
        }

        public virtual void AttachContext(TViewModel viewModel)
        {
            if (viewModel is BaseViewModel<TModel, TContext>)
                (viewModel as BaseViewModel<TModel, TContext>).Context = Context;
        }

        public virtual void AttachSubTypes(TViewModel viewModel)
        {
            foreach (var subTypeDefinition in SubTypeModelBinder.SubTypeDefinitions.Where(t => t.Item1 == typeof(TViewModel)))
            {
                viewModel.GetType()
                    .GetProperty(subTypeDefinition.Item2)
                    .SetValue(viewModel, Activator.CreateInstance(subTypeDefinition.Item4), null);
            }
        }

        public virtual TModel CreateModelInstance()
        {
            return Activator.CreateInstance<TModel>();
        }

        [HttpGet]
        public ActionResult Details(TKey id)
        {
            ViewData.Model = Set.Find(id);
            return View(DetailsViewName);
        }

        [HttpGet]
        public ActionResult Edit(TKey id)
        {
            var model = Set.Find(id);
            var viewModel = Activator.CreateInstance<TViewModel>();

            AttachSubTypes(viewModel);
            AttachContext(viewModel);
            
            viewModel.Load(model);
            viewModel.Databind();

            ViewData.AddSingle(Form.Edit());
            
            return View(EditViewName, viewModel);
        }

        [HttpPost]
        public ActionResult Edit(TKey id, TViewModel viewModel)
        {
            AttachContext(viewModel);         
            
            if (ModelState.IsValid)
            {
                var model = Set.Find(id);
                viewModel.Save(model);
                SaveChanges();

                return RedirectToAction("Details", new { id = id });
            }

            viewModel.Databind();

            ViewData.AddSingle(Form.Edit());

            return View(EditViewName, viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = CreateModelInstance();
            var viewModel = Activator.CreateInstance<TViewModel>();

            AttachSubTypes(viewModel);
            AttachContext(viewModel);

            viewModel.Load(model);
            viewModel.Databind();

            ViewData.AddSingle(Form.Create());

            return View(CreateViewName, viewModel);
        }

        [HttpPost]
        public virtual ActionResult Create(TViewModel viewModel)
        {
            AttachContext(viewModel);           
            
            if (ModelState.IsValid)
            {
                var model = CreateModelInstance();
                viewModel.Save(model);
                Set.Add(model);
                SaveChanges();

                return RedirectToAction("Details", new { id = model.GetKey() });
            }

            viewModel.Databind();

            ViewData.AddSingle(Form.Create());

            return View(CreateViewName, viewModel);
        }

        public virtual ICollection<IWebCommand> GetCommands(Query query)
        {
            return new List<IWebCommand>();
        }

        public ActionResult Command(TKey id, string commandName)
        {
            return CommandResult(GetCommands(null), commandName, Set.Find(id));
        }

        [HttpPost]
        public ActionResult Delete(TKey id)
        {
            var model = Set.Find(id);
            Set.Remove(model);
            SaveChanges();
            return RedirectToIndex();
        }

        public virtual void SaveChanges()
        {
            if (Context is DbContext)
            {
                (Context as DbContext).SaveChanges();
            }
            else if (Context is IDbContext)
            {
                (Context as IDbContext).SaveChanges();
            }
            else if (Context is IRepository<TModel>)
            {
                (Context as IRepository<TModel>).SaveChanges();
            }
        }
    }
}
