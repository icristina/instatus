using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Models
{
    public interface IUserGeneratedContent : ITimestamp
    {
        string Published { get; set; }
    }
}