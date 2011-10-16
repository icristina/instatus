using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Services;
using System.Data.Objects;
using System.Data.Services.Common;
using System.Data.Entity;
using System.Net;
using Instatus.Web;

namespace Instatus.Data
{
    public class BaseDataService<T> : DataService<ObjectContext> where T : DbContext, new()
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
        }

        protected override void HandleException(HandleExceptionArgs args)
        {
            base.HandleException(args);
        }

        protected override void OnStartProcessingRequest(ProcessRequestArgs args)
        {
            var user = HttpContext.Current.User;
            
            if (!user.Identity.IsAuthenticated)
                throw new DataServiceException((int)HttpStatusCode.Unauthorized, "Unauthorized");
            
            if(!user.IsInRole(WebRole.Administrator) && args.OperationContext.RequestMethod != "GET")
                throw new DataServiceException((int)HttpStatusCode.Unauthorized, "Unauthorized");

            base.OnStartProcessingRequest(args);
        }

        protected override ObjectContext CreateDataSource()
        {
            return new T()
                    .DisableProxiesAndLazyLoading()
                    .ObjectContext();
        }
    }
}