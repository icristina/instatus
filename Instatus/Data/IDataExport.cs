using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.ComponentModel.Composition;
using Instatus.Web;

namespace Instatus.Data
{
    public interface IDataExport
    {
        string Name { get; }
        IEnumerable Data { get; }
    }
}