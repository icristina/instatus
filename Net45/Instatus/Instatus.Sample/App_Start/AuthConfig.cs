﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Instatus.Core;
using Instatus.Integration.Server;
using Instatus.Core.Impl;
using Instatus.Integration.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Instatus.Sample.AuthConfig), "RegisterProviders")]

namespace Instatus.Sample
{
    public class AuthConfig
    {
        public static void RegisterProviders()
        {
            BaseMvcConfig.RegisterMembershipProvider();
            BaseMvcConfig.RegisterRoleProvider();
        }
    }
}