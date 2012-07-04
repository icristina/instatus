using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Instatus.Core
{
    public interface IMapper
    {
        Expression<Func<T, TOutput>> Projection<T, TOutput>() 
            where T : class 
            where TOutput : class;
        
        T Map<T>(object source) 
            where T : class;
        
        void Inject(object target, object source);
    }
}
