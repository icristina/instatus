using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;

namespace Instatus.Data
{
    public interface IDataImport
    {
        void ImportRow(IDbContext context, DataRow row);
    }
}