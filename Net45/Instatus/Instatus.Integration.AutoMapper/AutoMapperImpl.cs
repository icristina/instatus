using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Instatus.Core;
using autoMapper = AutoMapper;

namespace Instatus.Integration.AutoMapper
{
    public class AutoMapperImpl : IMapper
    {
        public Expression<Func<T, TOutput>> Projection<T, TOutput>() 
            where T : class
            where TOutput : class
        {
            return source => Map<TOutput>(source);
        }        
        
        public T Map<T>(object source) 
            where T : class
        {
            return autoMapper.Mapper.Map<T>(source);
        }

        public void Inject(object target, object source)
        {
            throw new NotImplementedException();
        }
    }
}
