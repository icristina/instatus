using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Web
{
    public class AllowUploadAttribute : Attribute
    {
        public bool Allow { get; set; }

        public AllowUploadAttribute()
        {
            Allow = true;
        }

        public AllowUploadAttribute(bool allow)
        {
            Allow = allow;
        }
    }
}