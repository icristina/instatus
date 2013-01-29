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
    public class EntityStorageApiController<TEntity, TModel> : ApiController
        where TEntity : class
        where TModel : class
    {
        private IEntityStorage entityStorage;
        private IMapper<TEntity, TModel> mapper;

        public virtual IQueryable<TModel> Get()
        {
            return entityStorage
                .Set<TEntity>()
                .Select(mapper.GetProjection());
        }

        [CheckId]
        public virtual HttpResponseMessage Get(int id)
        {
            var entity = entityStorage.Set<TEntity>().Find(id);

            if (entity == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var model = mapper.CreateViewModel(entity);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [CheckId, CheckModelForNull, CheckModelState]
        public virtual HttpResponseMessage Put(int id, TModel model)
        {
            if (id == (model as dynamic).Id)
            {
                var entity = entityStorage.Set<TEntity>().Find(id);

                mapper.FillEntity(entity, model);

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

        [CheckModelForNull, CheckModelState]
        public virtual HttpResponseMessage Post(TModel model)
        {
            var entity = mapper.CreateEntity(model);

            entityStorage.Set<TEntity>().Add(entity);
            entityStorage.SaveChanges();

            var response = Request.CreateResponse(HttpStatusCode.Created, model);

            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = (model as dynamic).Id }));

            return response;
        }

        [CheckId]
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

            var model = mapper.CreateViewModel(entity);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        public EntityStorageApiController(IEntityStorage entityStorage, IMapper<TEntity, TModel> mapper)
        {
            this.entityStorage = entityStorage;
            this.mapper = mapper;
        }
    }
}