using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Runtime.Serialization;
using System.IO;
using System.Web.Hosting;
using System.Collections;
using System.Data;
using Instatus.Data;
using System.Linq.Expressions;

namespace Instatus
{
    public static class DbContextExtensions
    {
        public static void ClearCollection<TEntity, TCollection>(this DbContext context, TEntity entity, Expression<Func<TEntity, ICollection<TCollection>>> accessor) where TEntity : class where TCollection : class
        {
                var entry = context.Entry(entity);
                var originalState = entry.State;

                if (originalState == EntityState.Deleted)
                    entry.State = EntityState.Unchanged;
                
                var collection = entry.Collection(accessor);
                var set = context.Set<TCollection>();    

                collection.Load();

                foreach(var association in collection.CurrentValue.ToList())
                    set.Remove(association);

                entry.State = originalState;
        }
        
        private static Type GetRootType(Type type)
        {
            while (type.BaseType != typeof(object)) // assumption, must be TPH inheritense
                type = type.BaseType;

            return type;
        }

        private static string GetEntitySetName(Type type)
        {
            return type.Name.ToPlural(); // assumption, entity set must be type.Name plural
        }

        public static object GetKey(this DbContext context, object entry)
        {
            var entitySetType = GetRootType(entry.GetType());
            var entitySetName = GetEntitySetName(entitySetType);

            return context.ObjectContext()
                    .CreateEntityKey(entitySetName, entry)
                    .EntityKeyValues[0].Value; // assumption, single key
        }        
        
        public static T DisableProxiesAndLazyLoading<T>(this T context) where T : DbContext
        {
            context.Configuration.LazyLoadingEnabled = false;
            context.Configuration.ProxyCreationEnabled = false;         
            return context;
        }

        public static ObjectContext ObjectContext(this DbContext context)
        {
            return ((IObjectContextAdapter)context).ObjectContext;
        }

        public static void MarkAssociationsDeleted<T>(this DbContext context, ICollection<T> entities)
        {
            entities.Clear();
        }

        public static void MarkDeleted<T>(this DbContext context, ICollection<T> entities) where T : class
        {
            entities.ToList().ForEach(e => context.MarkDeleted(e));
        }

        public static void MarkDeleted<T>(this DbContext context, T entity) where T : class
        {
            context.Entry(entity).State = EntityState.Deleted;
        }

        public static void LoadFromAppData(this DbContext context, IEnumerable<Type> knownTypes, IEnumerable<Type> seedTypes)
        {
            foreach (var type in seedTypes)
            {
                var xml = string.Format("~/App_Data/{0}.xml", type.Name.ToPlural());
                var listType = typeof(List<>).MakeGenericType(type);
                var entities = Generator.LoadXml(listType, xml, knownTypes);
                var set = context.Set(type);

                foreach (var entity in entities as IEnumerable)
                {
                    set.Add(entity);
                }
            }
        }
    }
}