using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Data
{
    public interface IUserGeneratedContent : ITimestamp
    {
#if NET45
        Published Published { get; set; }
#else
        string Published { get; set; }
#endif
    }
}