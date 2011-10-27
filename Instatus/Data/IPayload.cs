using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Data
{
    public interface IPayload
    {
        byte[] Data { get; set; }
    }
}