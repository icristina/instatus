using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Sample.Controllers
{
    public class AccountController : Instatus.Integration.Mvc.AccountController
    {
        public AccountController(IMembershipProvider membershipProvider, ICredentialStorage credentialStorage)
            : base(membershipProvider, credentialStorage)
        {

        }
    }
}
