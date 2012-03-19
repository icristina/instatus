﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Data
{
    public interface IJob<TContext, TResult> : INamed
    {
        string Name { get; }
        TResult Process(TContext context);
    }
}