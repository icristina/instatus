using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instatus.Core;
using autoMapper = AutoMapper;

namespace Instatus.Integration.AutoMapper
{
    public class AutoMapperImpl : IMapper
    {
        public T Map<T>(object source) where T : class
        {
            return autoMapper.Mapper.Map<T>(source);
        }

        public void Inject(object target, object source)
        {
            throw new NotImplementedException();
        }
    }
}
