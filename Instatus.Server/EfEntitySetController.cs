using Instatus.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

namespace Instatus.Server
{
    public abstract class EfEntitySetController<TContext, TEntity, TKey> 
        : EntitySetController<TEntity, int> 
        where TContext : DbContext, new()
        where TEntity : class
    {
        protected TContext Context = new TContext();
        
        [Queryable(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<TEntity> Get()
        {
            return Context.Set<TEntity>();
        }

        protected override TEntity GetEntityByKey(int key)
        {
            return Context.Set<TEntity>().Find(key);
        }

        protected override int GetKey(TEntity entity)
        {
            return (int)((dynamic)entity).Id;
        }

        protected override TEntity CreateEntity(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
            
            return entity;
        }

        protected override TEntity UpdateEntity(int key, TEntity update)
        {
            var existingEntity = GetEntityByKey(key);
            
            if (existingEntity == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Context.Set<TEntity>().Attach(update);
            Context.Entry(update).State = EntityState.Modified;
            Context.SaveChanges();

            return update;
        }

        protected override TEntity PatchEntity(int key, Delta<TEntity> patch)
        {
            var entity = GetEntityByKey(key);

            if (entity == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            patch.Patch(entity);

            Context.SaveChanges();

            return entity;
        }

        public override void Delete(int key)
        {
            var entity = GetEntityByKey(key);

            DeleteEntity(entity);
        }

        protected virtual void DeleteEntity(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            Context.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            Context.Dispose();
            
            base.Dispose(disposing);
        }
    }
}