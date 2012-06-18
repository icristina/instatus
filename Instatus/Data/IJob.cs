using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Data
{
    public interface IJob<TContext, TResult>
    {
        TResult Process(TContext context);
    }
}