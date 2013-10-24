using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Server
{
    public class LoginModel
    {
        public string Message { get; set; }
        public IEnumerable<AuthenticationDescription> Providers { get; set; }
        public string ReturnUrl { get; set; }
    }
}
