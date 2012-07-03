using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core.Impl
{
    public class SingleEntityMapper<TEntity, TModel> : IMapper 
        where TEntity : class 
        where TModel : class
    {
        private Func<TEntity, TModel> mapEntityToViewModel;
        private Func<TModel, TEntity> mapViewModelToEntity;
        private Action<TEntity, TModel> injectViewModelValuesToEntity;
        
        public T Map<T>(object source) where T : class
        {
            if (source is TEntity)
                return mapEntityToViewModel.Invoke((TEntity)source) as T;

            if (source is TModel)
                return mapViewModelToEntity.Invoke((TModel)source) as T;

            throw new NotSupportedException("No mapping exists");
        }

        public void Inject(object target, object source)
        {
            if (target is TEntity && source is TModel)
                injectViewModelValuesToEntity.Invoke(target as TEntity, source as TModel);

            throw new NotSupportedException("No injection exists");
        }

        public SingleEntityMapper(
            Func<TEntity, TModel> mapEntityToViewModel, 
            Func<TModel, TEntity> mapViewModelToEntity, 
            Action<TEntity, TModel> injectViewModelValuesToEntity)
        {
            this.mapEntityToViewModel = mapEntityToViewModel;
            this.mapViewModelToEntity = mapViewModelToEntity;
            this.injectViewModelValuesToEntity = injectViewModelValuesToEntity;
        }
    }
}
