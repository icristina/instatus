using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public enum Deployment
    {
        All,
        Development,
        QualityAssurance,
        Staging,
        Production
    }
}