using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Instatus.Models;
using Instatus.Services;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Instatus.Integration.Azure
{
    public class AzureLocalStorageService : FileSystemBlobService
    {
        public AzureLocalStorageService()
        {
            BasePath = RoleEnvironment.GetLocalResource(WebConstant.LocalResources.Output).RootPath;
        }
    }
}
