using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Instatus.Core;

namespace Instatus.Integration.Mvc
{
    public class EntityStorageController<TEntity> : EntityStorageController<TEntity, TEntity>
        where TEntity : class
    {
        public EntityStorageController(IEntityStorage entityStorage, IMapper mapper)
            : base(entityStorage, mapper)
        {

        }
    }    
    
    public class EntityStorageController<TEntity, TModel> : Controller
        where TEntity : class
        where TModel : class
    {
        private IEntityStorage entityStorage;
        private IMapper mapper;

        public ActionResult Index(int take = 20, int skip = 0)
        {
            IQueryable<TModel> queryable;
            
            if (typeof(TEntity) == typeof(TModel))
            {
                queryable = entityStorage.Set<TEntity>().Cast<TModel>();
            }
            else
            {
                queryable = entityStorage.Set<TEntity>().Select(mapper.Projection<TEntity, TModel>());
            }

            ViewData.Model = queryable.OrderBy(b => true).Take(take).Skip(skip).ToList();

            return View();
        }

        public ActionResult Details(int id)
        {
            var entity = entityStorage.Set<TEntity>().Find(id);

            if (entity == null)
            {
                return HttpNotFound();
            }

            ViewData.Model = mapper.Map<TModel>(entity);

            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewData.Model = Activator.CreateInstance<TModel>();

            return View();
        }

        [HttpPost]
        public ActionResult Create(TModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = mapper.Map<TEntity>(model);

                entityStorage.Set<TEntity>().Add(entity);
                entityStorage.SaveChanges();

                return RedirectToAction("Details", new { id = (entity as dynamic).Id });
            }
            else
            {
                ViewData.Model = model;
                
                return View();
            }            
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = entityStorage.Set<TEntity>().Find(id);

            if (entity == null)
            {
                return HttpNotFound();
            }

            ViewData.Model = mapper.Map<TModel>(entity);

            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, TModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = entityStorage.Set<TEntity>().Find(id);

                if (entity == null)
                {
                    return HttpNotFound();
                }

                mapper.Inject(entity, model);

                try
                {
                    entityStorage.SaveChanges();
                }
                catch
                {
                    return HttpNotFound();
                }

                return RedirectToAction("Details", new { id = id } );
            }
            else
            {
                ViewData.Model = model;
                
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var entity = entityStorage.Set<TEntity>().Find(id);

            if (entity == null)
            {
                return HttpNotFound();
            }

            entityStorage.Set<TEntity>().Delete(id);

            try
            {
                entityStorage.SaveChanges();
            }
            catch
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index");
        }

        public EntityStorageController(IEntityStorage entityStorage, IMapper mapper)
        {
            this.entityStorage = entityStorage;
            this.mapper = mapper;
        }
    }
}
