using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Instatus.Core;
using Omu.ValueInjecter;

namespace Instatus.Integration.ValueInjecter
{
    public class ValueInjecterMapper : IMapper
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
            if (source is T)
                return (T)source;
            
            var output = Activator.CreateInstance<T>();
            output.InjectFrom(source);
            return output;
        }

        public void Inject(object target, object source)
        {
            target.InjectFrom(source);
        }
    }
}
