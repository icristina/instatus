﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Core
{
    public interface IMessaging
    {
        void Send(string from, string to, string subject, string body, IMetadata metaData);
    }
}
