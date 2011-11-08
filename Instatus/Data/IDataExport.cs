using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Instatus.Data
{
    public interface IDataExport
    {
        string Name { get; }
        IEnumerable Data { get; }
    }
}