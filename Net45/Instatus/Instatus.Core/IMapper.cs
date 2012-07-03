using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface IMapper
    {
        T Map<T>(object source) where T : class;
        void Inject(object target, object source);
    }
}
