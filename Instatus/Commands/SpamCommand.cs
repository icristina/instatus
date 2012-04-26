﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Data;
using Instatus.Web;
using Instatus.Models;
using System.Web.Routing;
using System.Collections.Specialized;

namespace Instatus.Commands
{
    public class SpamCommand<T> : StatusCommand<T> where T : class, IUserGeneratedContent
    {
        public SpamCommand()
            : base(Published.Spam, WebPhrase.Flag("spam"), Published.Active, WebPhrase.RemoveFlag("spam"))
        {

        }
    }
}
