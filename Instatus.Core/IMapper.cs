using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Instatus.Core
{
    public interface IMapper<TEntity, TViewModel>
    {
        Expression<Func<TEntity, TViewModel>> GetProjection();
        TEntity CreateEntity(TViewModel model);
        TViewModel CreateViewModel(TEntity entity);
        void FillEntity(TEntity entity, TViewModel model);
    }
}
