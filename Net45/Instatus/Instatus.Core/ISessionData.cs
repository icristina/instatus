﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Instatus.Core
{
    public interface ISessionData
    {
        string SiteName { get; }
        string Locale { get; set; }
    }
}
