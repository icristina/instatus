using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Entities;
using Instatus.Models;

namespace Instatus.Services
{
    public class DbCredentialService : ICredentialService
    {
        private IApplicationModel applicationModel;
        
        public ICredential GetCredential(Provider provider)
        {
            return applicationModel.Credentials.Where(FilterBy.Provider(provider)).FirstOrDefault();
        }

        public DbCredentialService(IApplicationModel applicationModel)
        {
            this.applicationModel = applicationModel;
        }
    }
}