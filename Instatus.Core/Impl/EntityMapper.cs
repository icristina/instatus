using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Instatus.Core.Impl
{
    public abstract class EntityMapper<TEntity, TModel> : IMapper 
        where TEntity : class 
        where TModel : class
    {
        public abstract Expression<Func<TEntity, TModel>> GetProjection();
        public abstract TEntity CreateEntity(TModel model);
        public abstract TModel CreateViewModel(TEntity entity);
        public abstract void FillEntity(TEntity entity, TModel model);

        public Expression<Func<T, TOutput>> Projection<T, TOutput>()
            where T : class
            where TOutput : class
        {
            if (typeof(T) == typeof(TEntity) && typeof(TOutput) == typeof(TModel))
                return GetProjection() as Expression<Func<T, TOutput>>;

            throw new NotSupportedException("No mapping exists");
        }

        public T Map<T>(object source) where T : class
        {
            if (source is TEntity)
                return CreateViewModel((TEntity)source) as T;

            if (source is TModel)
                return CreateEntity((TModel)source) as T;

            throw new NotSupportedException("No mapping exists");
        }

        public void Inject(object target, object source)
        {
            if (target is TEntity && source is TModel)
            {
                FillEntity(target as TEntity, source as TModel);
            }
            else
            {
                throw new NotSupportedException("No injection exists");
            }
        }
    }
}
