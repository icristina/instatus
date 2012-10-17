using Instatus.Core;
using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Sample.Controllers
{
    public class AccountController : Instatus.Integration.Mvc.AccountController
    {
        public AccountController(IMembership membership, IKeyValueStorage<Credential> credentials)
            : base(membership, credentials)
        {

        }
    }
}
