using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Instatus.Integration.Server
{
    // http://www.asp.net/web-api/overview/security/basic-authentication
    public class BasicAuthHttpModule : IHttpModule
    {
        private const string Realm = "Web";

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }

        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;

            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private static bool AuthenticateUser(string credentials)
        {
            var validated = false;

            try
            {
                var encoding = Encoding.GetEncoding("iso-8859-1");

                credentials = encoding.GetString(Convert.FromBase64String(credentials));

                var separator = credentials.IndexOf(':');
                var userName = credentials.Substring(0, separator);
                var password = credentials.Substring(separator + 1);

                using (var container = AppContext.CreateContainer())
                {
                    var membershipProvider = container.Resolve<IMembership>();

                    validated = membershipProvider.ValidateUser(userName, password);
                }

                if (validated)
                {
                    var identity = new GenericIdentity(userName);

                    SetPrincipal(new GenericPrincipal(identity, null));
                }
            }
            catch (FormatException)
            {
                validated = false;
            }

            return validated;
        }

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
            var authHeader = request.Headers["Authorization"];

            if (authHeader != null)
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                if (authHeaderVal.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && authHeaderVal.Parameter != null)
                {
                    AuthenticateUser(authHeaderVal.Parameter);
                }
            }
        }

        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;

            if (response.StatusCode == 401)
            {
                response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", Realm));
            }
        }

        public void Dispose()
        {

        }
    }
}
