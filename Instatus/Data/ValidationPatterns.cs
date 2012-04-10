using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instatus.Data
{
    public class ValidationPatterns
    {
        public const string EmailAddress = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        public const string Slug = @"^[a-z0-9-]+$";
        public const string RealName = @"^[A-Za-z-\s]+$";
    }
}