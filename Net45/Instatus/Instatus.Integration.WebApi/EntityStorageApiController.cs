using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Net.Http;
using Instatus.Core;
using System.Net;
using System.Linq.Expressions;

namespace Instatus.Integration.WebApi
{
    public class EntityStorageApiController<TEntity> : EntityStorageApiController<TEntity, TEntity>
        where TEntity : class
    {
        public EntityStorageApiController(IEntityStorage entityStorage, IMapper mapper)
            : base(entityStorage, mapper)
        {

        }
    }
    
    public class EntityStorageApiController<TEntity, TModel> : ApiController 
        where TEntity : class
        where TModel : class
    {
        private IEntityStorage entityStorage;
        private IMapper mapper;

        public virtual IQueryable<TModel> Get()
        {
            if (typeof(TEntity) == typeof(TModel))
            {
                return entityStorage.Set<TEntity>() as IQueryable<TModel>;
            }
            else
            {
                return entityStorage.Set<TEntity>().Select(mapper.Projection<TEntity, TModel>());
            }
        }

        public virtual HttpResponseMessage Get(int id)
        {
            var entity = entityStorage.Set<TEntity>().Find(id);
            
            if (entity == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var model = mapper.Map<TModel>(entity);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        public virtual HttpResponseMessage Put(int id, TModel model)
        {
            if (ModelState.IsValid && id == (model as dynamic).Id)
            {
                var entity = entityStorage.Set<TEntity>().Find(id);

                mapper.Inject(entity, model);

                try
                {
                    entityStorage.SaveChanges();
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        public virtual HttpResponseMessage Post(TModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = mapper.Map<TEntity>(model);
                
                entityStorage.Set<TEntity>().Add(entity);
                entityStorage.SaveChanges();

                var response = Request.CreateResponse(HttpStatusCode.Created, model);
                
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = (model as dynamic).Id }));
                
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        public virtual HttpResponseMessage Delete(int id)
        {
            var entity = entityStorage.Set<TEntity>().Find(id);
            
            if (entity == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            entityStorage.Set<TEntity>().Delete(id);

            try
            {
                entityStorage.SaveChanges();
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var model = mapper.Map<TModel>(entity);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        public EntityStorageApiController(IEntityStorage entityStorage, IMapper mapper)
        {
            this.entityStorage = entityStorage;
            this.mapper = mapper;
        }
    }
}
