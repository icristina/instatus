using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Models;

namespace Instatus.Services
{
    public interface ICredentialService
    {
        ICredential GetCredential(Provider provider);
    }
}