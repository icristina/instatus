using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Instatus.Core;
using System.Web.SessionState;

namespace Instatus.Integration.Mvc
{
    public abstract class EntityStorageController<TEntity> : EntityStorageController<TEntity, TEntity>
        where TEntity : class
    {
        public EntityStorageController(IEntityStorage entityStorage, IMapper mapper)
            : base(entityStorage, mapper)
        {

        }
    }

    [SessionState(SessionStateBehavior.Disabled)]
    public abstract class EntityStorageController<TEntity, TViewModel> : Controller
        where TEntity : class
        where TViewModel : class
    {
        public IEntityStorage EntityStorage { get; private set; }
        public IMapper Mapper { get; private set; }

        public IEntitySet<TEntity> EntitySet
        {
            get
            {
                return EntityStorage.Set<TEntity>();
            }
        }

        public virtual IOrderedQueryable<TViewModel> Query(string orderBy, string filter) 
        {
            IQueryable<TViewModel> queryable;

            if (typeof(TEntity) == typeof(TViewModel))
            {
                queryable = EntitySet.Cast<TViewModel>();
            }
            else
            {
                queryable = EntitySet.Select(Mapper.Projection<TEntity, TViewModel>());
            }

            return queryable.OrderBy(b => true);
        }

        public virtual ActionResult Index(
            string orderBy, 
            string filter, 
            int pageIndex = 0, 
            int pageSize = 20)
        {
            ViewData.Model = new PagedViewModel<TViewModel>(Query(orderBy, filter), pageIndex, pageSize);

            return View();
        }

        public ActionResult Details(int id)
        {
            var entity = EntitySet.Find(id);

            if (entity == null)
            {
                return HttpNotFound();
            }

            ViewData.Model = Mapper.Map<TViewModel>(entity);

            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewData.Model = Activator.CreateInstance<TViewModel>();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = Mapper.Map<TEntity>(model);

                EntitySet.Add(entity);
                EntityStorage.SaveChanges();

                OnCreated(entity);

                return RedirectToAction("Details", new { id = (entity as dynamic).Id });
            }
            else
            {
                ViewData.Model = model;

                return View();
            }
        }

        public virtual void OnCreated(TEntity entity)
        {

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = EntitySet.Find(id);

            if (entity == null)
            {
                return HttpNotFound();
            }

            ViewData.Model = Mapper.Map<TViewModel>(entity);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = EntitySet.Find(id);

                if (entity == null)
                {
                    return HttpNotFound();
                }

                Mapper.Inject(entity, model);

                try
                {
                    EntityStorage.SaveChanges();

                    OnEdited(entity);
                }
                catch
                {
                    return HttpNotFound();
                }

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                ViewData.Model = model;

                return View();
            }
        }

        public virtual void OnEdited(TEntity entity)
        {

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var entity = EntitySet.Find(id);

            if (entity == null)
            {
                return HttpNotFound();
            }

            EntitySet.Delete(id);

            try
            {
                EntityStorage.SaveChanges();

                OnDeleted(entity);
            }
            catch
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index");
        }

        public virtual void OnDeleted(TEntity entity)
        {

        }

        public EntityStorageController(IEntityStorage entityStorage, IMapper mapper)
        {
            EntityStorage = entityStorage;
            Mapper = mapper;
        }
    }
}
