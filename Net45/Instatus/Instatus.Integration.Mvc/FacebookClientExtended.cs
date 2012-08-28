using DotNetOpenAuth.AspNet.Clients;
using Instatus.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Instatus.Integration.Mvc
{
    // https://github.com/AArnott/dotnetopenid/blob/master/src/DotNetOpenAuth.AspNet/Clients/OAuth2/FacebookClient.cs
    public class FacebookClientExtended : OAuth2Client
    {
        private const string AuthorizationEndpoint = "https://www.facebook.com/dialog/oauth";
        private const string TokenEndpoint = "https://graph.facebook.com/oauth/access_token";
        private readonly string appId;
        private readonly string appSecret;
        private readonly string[] claims;

        public FacebookClientExtended(string appId, string appSecret, string[] claims)
            : base("facebook")
        {
            this.appId = appId;
            this.appSecret = appSecret;
            this.claims = claims;
        }

        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            return new PathBuilder(AuthorizationEndpoint)
                .Query("client_id", appId)
                .Query("redirect_uri", returnUrl.AbsoluteUri)
                .Query("scope", claims == null ? "" : string.Join(",", claims))
                .ToUri();
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString("https://graph.facebook.com/me?access_token=" + accessToken);
                return JsonConvert.DeserializeObject<IDictionary<string, object>>(json)
                    .ToDictionary(pair => pair.Key, pair=> pair.Value.ToString());
            }
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            var pathBuilder = new PathBuilder(TokenEndpoint)
                .Query("client_id", appId)
                .Query("redirect_uri", NormalizeHexEncoding(returnUrl.AbsoluteUri))
                .Query("client_secret", appSecret)
                .Query("code", authorizationCode);

            using (var client = new WebClient())
            {
                var data = client.DownloadString(pathBuilder.ToUri());
                
                if (string.IsNullOrEmpty(data))
                {
                    return null;
                }

                var parsedQueryString = HttpUtility.ParseQueryString(data);

                return parsedQueryString["access_token"];
            }
        }

        private static string NormalizeHexEncoding(string url)
        {
            var chars = url.ToCharArray();
            for (int i = 0; i < chars.Length - 2; i++)
            {
                if (chars[i] == '%')
                {
                    chars[i + 1] = char.ToUpperInvariant(chars[i + 1]);
                    chars[i + 2] = char.ToUpperInvariant(chars[i + 2]);
                    i += 2;
                }
            }
            return new string(chars);
        }
    }
}
