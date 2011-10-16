using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Models
{
    public interface IUserGeneratedContent
    {
        string Status { get; set; }
        DateTime CreatedTime { get; set; }
        User User { get; set; }
    }
}