using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Data
{
    public interface IObjectMapping<TInput, TOutput>
    {
        void Map(TInput input, TOutput output);
    }
}