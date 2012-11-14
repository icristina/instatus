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
        public Expression<Func<TEntity, TModel>> ProjectEntityToViewModel { get; protected set; }
        public Func<TEntity, TModel> MapEntityToViewModel { get; protected set; }
        public Func<TModel, TEntity> MapViewModelToEntity { get; protected set; }
        public Action<TEntity, TModel> InjectViewModelValuesToEntity { get; protected set; }

        public Expression<Func<T, TOutput>> Projection<T, TOutput>()
            where T : class
            where TOutput : class
        {
            if (typeof(T) == typeof(TEntity) && typeof(TOutput) == typeof(TModel))
                return ProjectEntityToViewModel as Expression<Func<T, TOutput>>;

            throw new NotSupportedException("No mapping exists");
        }

        public T Map<T>(object source) where T : class
        {
            if (source is TEntity)
                return MapEntityToViewModel.Invoke((TEntity)source) as T;

            if (source is TModel)
                return MapViewModelToEntity.Invoke((TModel)source) as T;

            throw new NotSupportedException("No mapping exists");
        }

        public void Inject(object target, object source)
        {
            if (target is TEntity && source is TModel)
            {

                InjectViewModelValuesToEntity.Invoke(target as TEntity, source as TModel);
            }
            else
            {
                throw new NotSupportedException("No injection exists");
            }
        }

        public EntityMapper() { }

        public EntityMapper(
            Expression<Func<TEntity, TModel>> projectEntityToViewModelForQuery, 
            Func<TEntity, TModel> mapEntityToViewModelForSingle,
            Func<TModel, TEntity> mapViewModelToEntity, 
            Action<TEntity, TModel> injectViewModelValuesToEntity)
        {
            this.ProjectEntityToViewModel = projectEntityToViewModelForQuery;
            this.MapEntityToViewModel = mapEntityToViewModelForSingle ?? projectEntityToViewModelForQuery.Compile();
            this.MapViewModelToEntity = mapViewModelToEntity;
            this.InjectViewModelValuesToEntity = injectViewModelValuesToEntity;
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
