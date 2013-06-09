using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Instatus.Server
{
    public class MethodOverrideHandler : DelegatingHandler
    {
        private readonly string[] methods = { "DELETE", "HEAD", "PUT", "PATCH" };
        private const string header = "X-HTTP-Method-Override";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Post && request.Headers.Contains(header))
            {
                var method = request.Headers.GetValues(header).FirstOrDefault();
                
                if (methods.Contains(method, StringComparer.InvariantCultureIgnoreCase))
                {
                    request.Method = new HttpMethod(method);
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
