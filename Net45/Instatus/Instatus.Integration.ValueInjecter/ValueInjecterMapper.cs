using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Core;
using Omu.ValueInjecter;

namespace Instatus.Integration.ValueInjecter
{
    public class ValueInjecterMapper : IMapper
    {
        public T Map<T>(object source) where T : class
        {
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
