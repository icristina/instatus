using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Dynamic;

namespace Instatus.Data
{
    public class Record<TModel> : Record<TModel, object>
    {

    }

    public class Record<TModel, TMetadata>
    {
        public TModel Item { get; set; }
        public TMetadata Metadata { get; set; }
        public object Associations { get; set; }
    }
}