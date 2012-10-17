using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Instatus.Integration.WebApi
{
    public class HttpMethodOverrideHandler : DelegatingHandler
    {
        private HttpMethod[] supportedMethods;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var method = request.Headers
                    .Where(h => h.Key == "X-HTTP-Method-Override")
                    .SelectMany(h => h.Value)
                    .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(method))
            {
                var overrideMethod = supportedMethods.FirstOrDefault(m => string.Equals(method, m.Method, StringComparison.OrdinalIgnoreCase));

                if (overrideMethod != null)
                {
                    request.Method = overrideMethod;
                }
            }

            return base.SendAsync(request, cancellationToken);
        }

        public HttpMethodOverrideHandler(HttpMethod[] supportedMethods)
        {
            this.supportedMethods = supportedMethods;
        }
    }
}