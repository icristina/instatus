using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Instatus.Web
{
    public enum WebNamespace
    {
        [Description("html")]
        Html,
        [Description("og")]
        OpenGraph
    }
}