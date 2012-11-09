using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Integration.Mvc
{
    // http://www.tumblr.com/docs/en/api/v2#auth
    // https://github.com/DotNetOpenAuth/DotNetOpenAuth/blob/master/src/DotNetOpenAuth.AspNet/Clients/OAuth/TwitterClient.cs
    public class TumblrOAuthClient : OAuthClient 
    {
        public static readonly ServiceProviderDescription TumblrServiceDescription = new ServiceProviderDescription
        {
            RequestTokenEndpoint =
                new MessageReceivingEndpoint(
                    "http://www.tumblr.com/oauth/request_token",
                    HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
            UserAuthorizationEndpoint =
                new MessageReceivingEndpoint(
                    "http://www.tumblr.com/oauth/authorize",
                    HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
            AccessTokenEndpoint =
                new MessageReceivingEndpoint(
                    "http://www.tumblr.com/oauth/access_token",
                    HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
            TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
        };

        public TumblrOAuthClient(string consumerKey, string consumerSecret)
			: this(consumerKey, consumerSecret, new AuthenticationOnlyCookieOAuthTokenManager()) { }

		public TumblrOAuthClient(string consumerKey, string consumerSecret, IOAuthTokenManager tokenManager)
			: base("tumblr", TumblrServiceDescription, new SimpleConsumerTokenManager(consumerKey, consumerSecret, tokenManager)) {
		}

        protected override AuthenticationResult VerifyAuthenticationCore(AuthorizedTokenResponse response)
        {
            var accessToken = response.AccessToken;
            var userId = response.ExtraData["user_id"];
            var userName = response.ExtraData["screen_name"];
            var extraData = new Dictionary<string, string>();
            
            return new AuthenticationResult(true, ProviderName, userId, userName, extraData);
        }
    }
}
