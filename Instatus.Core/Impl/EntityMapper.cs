using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Instatus.Core.Impl
{
    public class EntityMapper<TEntity, TModel> : IMapper 
        where TEntity : class 
        where TModel : class
    {
        private Expression<Func<TEntity, TModel>> projectEntityToViewModelForSelect;
        private Func<TEntity, TModel> mapEntityToViewModelForSingle;
        private Func<TModel, TEntity> mapViewModelToEntity;
        private Action<TEntity, TModel> injectViewModelValuesToEntity;

        public Expression<Func<T, TOutput>> Projection<T, TOutput>()
            where T : class
            where TOutput : class
        {
            if (typeof(T) == typeof(TEntity) && typeof(TOutput) == typeof(TModel))
                return projectEntityToViewModelForSelect as Expression<Func<T, TOutput>>;

            throw new NotSupportedException("No mapping exists");
        }

        public T Map<T>(object source) where T : class
        {
            if (source is TEntity)
                return mapEntityToViewModelForSingle.Invoke((TEntity)source) as T;

            if (source is TModel)
                return mapViewModelToEntity.Invoke((TModel)source) as T;

            throw new NotSupportedException("No mapping exists");
        }

        public void Inject(object target, object source)
        {
            if (target is TEntity && source is TModel)
            {

                injectViewModelValuesToEntity.Invoke(target as TEntity, source as TModel);
            }
            else
            {
                throw new NotSupportedException("No injection exists");
            }
        }

        public EntityMapper(
            Expression<Func<TEntity, TModel>> projectEntityToViewModelForQuery, 
            Func<TEntity, TModel> mapEntityToViewModelForSingle,
            Func<TModel, TEntity> mapViewModelToEntity, 
            Action<TEntity, TModel> injectViewModelValuesToEntity)
        {
            this.projectEntityToViewModelForSelect = projectEntityToViewModelForQuery;
            this.mapEntityToViewModelForSingle = mapEntityToViewModelForSingle ?? projectEntityToViewModelForQuery.Compile();
            this.mapViewModelToEntity = mapViewModelToEntity;
            this.injectViewModelValuesToEntity = injectViewModelValuesToEntity;
        }

        public EntityMapper(
            Expression<Func<TEntity, TModel>> projectEntityToViewModelForQuery,
            Func<TModel, TEntity> mapViewModelToEntity,
            Action<TEntity, TModel> injectViewModelValuesToEntity)
            : this(projectEntityToViewModelForQuery, null, mapViewModelToEntity, injectViewModelValuesToEntity)
        {

        }

        public EntityMapper(
            Expression<Func<TEntity, TModel>> projectEntityToViewModelForQuery)
            : this(projectEntityToViewModelForQuery, null, null, null)
        {

        }
    }
}
