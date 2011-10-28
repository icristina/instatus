using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using Instatus.Web;

namespace Instatus
{
    public static class BooleanExtensions
    {
        public static string ToDescriptiveString(this bool value)
        {
            return value ? WebLocalization.Yes : WebLocalization.No;
        }
    }
}