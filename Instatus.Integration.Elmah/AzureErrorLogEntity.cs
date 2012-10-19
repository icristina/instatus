using Instatus.Core;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Integration.Elmah
{
    public class AzureErrorLogEntity : TableServiceEntity, ICreated
    {
        public DateTime Created { get; set; }
        public string Xml { get; set; }
    }
}
