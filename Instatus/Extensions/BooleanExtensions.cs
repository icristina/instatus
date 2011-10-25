using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Instatus
{
    public static class BooleanExtensions
    {
        public static string ToDescriptiveString(this bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}