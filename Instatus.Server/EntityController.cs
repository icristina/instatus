using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Instatus.Server
{
    public abstract class EntityController<TContext, TEntity, TModel> : Controller
        where TContext : DbContext, new()
        where TEntity : class, new()
        where TModel : new()
    {
        private TContext context;

        protected TContext Context
        {
            get
            {
                LazyInitializer.EnsureInitialized(ref context, () => new TContext());

                return context;
            }
        }

        protected virtual async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<ActionResult> Index()
        {
            AddPermissions();

            ViewData.Model = await GetAsync();

            return View();
        }

        protected virtual void AddPermissions()
        {
            ViewBag.CanRead = new Func<dynamic, bool>(e => CanRead(e));
            ViewBag.CanEdit = new Func<dynamic, bool>(e => CanEdit(e));
            ViewBag.CanCreate = new Func<bool>(() => CanCreate());
            ViewBag.CanDelete = new Func<dynamic, bool>(e => CanDelete(e));
        }

        public async Task<ActionResult> Details(int id)
        {
            var entity = await GetEntityByKey(id);

            if (entity == null)
            {
                return HttpNotFound();
            }

            if (!CanRead(entity))
            {
                return new HttpUnauthorizedResult();
            }

            ViewData.Model = entity;

            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!CanCreate())
            {
                return new HttpUnauthorizedResult();
            }

            ViewData.Model = CreateModel();

            OnCreating();

            return View();
        }

        protected virtual void OnCreating()
        {

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TModel model)
        {
            if (!CanCreate())
            {
                return new HttpUnauthorizedResult();
            }

            if (ModelState.IsValid)
            {
                var entity = CreateEntity(model);

                Context.Set<TEntity>().Add(entity);

                await Context.SaveChangesAsync();

                return OnCreated(entity);
            }
            else
            {
                ViewData.Model = model;

                OnCreating();

                return View();
            }
        }

        protected virtual ActionResult OnCreated(TEntity entity)
        {
            return RedirectToAction("Details", new { id = GetKey(entity) });
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var entity = await GetEntityByKey(id);

            if (entity == null)
            {
                return HttpNotFound();
            }

            if (!CanEdit(entity))
            {
                return new HttpUnauthorizedResult();
            }

            ViewData.Model = MapToModel(entity);

            OnEditing();

            return View();
        }

        protected virtual void OnEditing()
        {

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, TModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = await GetEntityByKey(id);

                if (!CanEdit(entity))
                {
                    return new HttpUnauthorizedResult();
                }

                UpdateEntity(model, entity);

                await Context.SaveChangesAsync();

                return OnEdited(entity);
            }
            else
            {
                ViewData.Model = model;

                OnEditing();

                return View();
            }
        }

        protected virtual ActionResult OnEdited(TEntity entity)
        {
            return RedirectToAction("Details", new { id = GetKey(entity) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await GetEntityByKey(id);

            if (entity == null)
            {
                return HttpNotFound();
            }

            if (!CanDelete(entity))
            {
                return new HttpUnauthorizedResult();
            }

            Delete(entity);

            await Context.SaveChangesAsync();

            return OnDeleted(entity);
        }

        protected virtual ActionResult OnDeleted(TEntity entity)
        {
            return RedirectToAction("Index");
        }

        protected virtual void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        protected virtual TModel MapToModel(TEntity source)
        {
            return Mapper.Map<TModel>(source);
        }

        protected virtual TEntity CreateEntity(TModel source)
        {
            return Mapper.Map(source, new TEntity());
        }

        protected virtual void UpdateEntity(TModel source, TEntity destination)
        {
            Mapper.Map(source, destination);
        }

        protected virtual TModel CreateModel()
        {
            return Mapper.Map(new TEntity(), new TModel());
        }

        protected int GetKey(TEntity entity)
        {
            return (int)((dynamic)entity).Id;
        }

        protected async Task<TEntity> GetEntityByKey(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && context != null)
            {
                context.Dispose();
                context = null;
            }

            base.Dispose(disposing);
        }

        protected ActionResult CloseResult()
        {
            return new ClientCloseResult();
        }

        protected virtual bool CanRead(dynamic entity)
        {
            return true;
        }

        protected virtual bool CanEdit(dynamic entity)
        {
            return true;
        }

        protected virtual bool CanCreate()
        {
            return true;
        }

        protected virtual bool CanDelete(dynamic entity)
        {
            return true;
        }

        public EntityController()
        {
            if (Mapper.FindTypeMapFor<TEntity, TModel>() == null)
            {
                Mapper.CreateMap<TEntity, TModel>();
            }

            if (Mapper.FindTypeMapFor<TModel, TEntity>() == null)
            {
                Mapper.CreateMap<TModel, TEntity>();
            }
        }

        internal class ClientCloseResult : ActionResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                var response = context.RequestContext.HttpContext.Response;

                response.ContentType = "text/html";
                response.Write("<script>window.opener.location.reload();window.close();</script>");
            }
        }
    }
}
