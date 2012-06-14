using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Helpers;
using System.Web.Security;
using Instatus;
using Instatus.Data;
using Instatus.Models;
using Instatus.Web;
using System.Web.Routing;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity;
using System.Configuration;
using Instatus.Services;
using System.Web.Mvc;
using Instatus.Entities;

namespace Instatus.Areas.Facebook
{
    public static class Facebook
    {
        static Facebook()
        {
            WebApp.OnReset(() => credential = null);
        }
        
        private static Uri Route(object resource, object connection)
        {
            var route = connection.IsEmpty() ?
                string.Format("https://graph.facebook.com/{0}", resource) :
                string.Format("https://graph.facebook.com/{0}/{1}", resource, connection.ToString().ToLower());
            
            return new Uri(route);
        }

        private static string InternalRequest(object resource, string accessToken = null, object connection = null)
        {
            Uri uri;

            if (!Uri.TryCreate(resource.ToString(), UriKind.Absolute, out uri))
            {
                uri = Route(resource, connection);
            }

            uri = uri.Query("access_token", accessToken);
            
            using (var client = new WebClient())
            {
                try
                {
                    return client.DownloadString(uri);
                }
                catch
                {
                    return null;
                }
            }
        }

        public static string Channel()
        {
            return "<script src='//connect.facebook.net/en_US/all.js'></script>";
        }

        public static string LikeButton(string uri = null, EmbedType embedType = EmbedType.Html, int width = 90)
        {
            uri = uri ?? HttpContext.Current.Request.Url.AbsoluteUri;
            
            TagBuilder tagBuilder;
            
            if(embedType == EmbedType.Html && Credential.HasFeature("xfbml")) {
                tagBuilder = new TagBuilder("div");

                tagBuilder.AddCssClass("fb-like");
                
                tagBuilder
                      .MergeDataAttribute("href", uri)
                      .MergeDataAttribute("send", "false")
                      .MergeDataAttribute("layout", "button_count")
                      .MergeDataAttribute("width", width)
                      .MergeDataAttribute("show-faces", "false");
 
            } else {
                tagBuilder = new TagBuilder("iframe");

                // extended_social_context=false attempts to disable "Be the first..." type descriptive text
                tagBuilder.MergeAttribute("src", string.Format("//www.facebook.com/plugins/like.php?href={0}&send=false&layout=button_count&width={1}&show_faces=false&action=like&colorscheme=light&font&height=21&appId={2}&extended_social_context=false", HttpUtility.UrlEncode(uri), width, Facebook.ApplicationId));
                tagBuilder.MergeAttribute("scrolling", "no");
                tagBuilder.MergeAttribute("frameborder", "0");
                tagBuilder.MergeAttribute("style", string.Format("border:none; overflow:hidden; width:{0}px; height:21px;", width));
                tagBuilder.MergeAttribute("allowTransparency", "true");
            }

            return tagBuilder.ToString();
        }

        public static string Picture(object resource, PictureSize size)
        {
            return string.Format("https://graph.facebook.com/{0}/picture?type={1}", resource, size.ToString().ToLower());
        }

        public static string Profile(object resource)
        {
            return string.Format("https://www.facebook.com/{0}", resource);
        }

        public static string Permalink(object resource)
        {
            var id = resource.ToString();
            var segments = id.Split('_');

            return segments.Length == 2 ?
                string.Format("http://www.facebook.com/{0}/posts/{1}", segments[0], segments[1]) :
                "http://www.facebook.com";
        }

        public static dynamic Request(object resource, string accessToken = null, object connection = null)
        {
            var response = InternalRequest(resource, accessToken, connection);
            return Json.Decode(response);
        }

        public static T Request<T>(object resource, string accessToken = null, object connection = null)
        {
            var response = InternalRequest(resource, accessToken, connection);
            return (T)Json.Decode(response, typeof(T));
        }

        public static void Publish(object graph, object resource, string accessToken = null, object connection = null)
        {
            graph.IsValid(true);
            
            var uri = Route(resource, connection)
                        .Query("access_token", accessToken);
            var values = graph.ToNameValueCollection();

            using (var client = new WebClient())
            {
                client.UploadValues(uri, values);
            }
        }

        public static List<Entry> Feed(object resource, string accessToken, Connection connection = Connection.Feed)
        {
            var response = Request(resource, accessToken, connection);
            var feed = new List<Entry>();

            foreach (var entry in response.data)
            {               
                feed.Add(new Entry()
                {
                    Picture = Facebook.Picture(entry.from.id, PictureSize.Square),
                    Kind = (entry.type as string).ToCapitalized(),
                    Source = "Facebook",
                    Title = entry.name,
                    Description = entry.message,
                    CreatedTime = DateTime.Parse(entry.created_time),
                    // Uri = entry.actionsentry.actions[0].link
                    Uri = Facebook.Permalink(entry.id)
                });
            }

            return feed;
        }

        private static Credential credential;

        public static Credential Credential
        {
            get
            {               
                if (credential.IsEmpty())
                {
                    var credentialService = WebApp.GetService<ICredentialService>();

                    credential = credentialService.GetCredential(Provider.Facebook) as Credential;
                }

                return credential;
            }
        }

        public static ICollection<string> Scope
        {
            get
            {
                return Credential.Scope.ToList();
            }
        }

        public static string ApplicationAlias
        {
            get
            {
                return Credential.Alias;
            }
        }

        public static string ApplicationSecret
        {
            get
            {
                return Credential.Secret;
            }
        }

        public static string ApplicationId
        {
            get
            {
                return Credential.Key;
            }
        }

        public static string CanvasUrl
        {
            get
            {
                return string.Format("https://apps.facebook.com/{0}", ApplicationAlias);
            }
        }

        private static string verificationToken;

        public static string VerificationToken
        {
            get
            {
                if (verificationToken.IsEmpty())
                    verificationToken = Guid.NewGuid().ToString();

                return verificationToken;
            }
        }

        public static string ApplicationAccessToken(string applicationId = null, string applicationSecret = null)
        {
            var uri = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&client_secret={1}&grant_type=client_credentials",
                applicationId ?? Facebook.ApplicationId,
                applicationSecret ?? Facebook.ApplicationSecret);

            using (var webClient = new WebClient()) 
            {
                return webClient.DownloadString(uri).SubstringAfter("access_token=");
            }
        }

        public static void AppRequest(object resource, string message, string applicationAccessToken = null)
        {
            var uri = string.Format("https://graph.facebook.com/{0}/apprequests?message='{1}'&data=''&{2}&method=post",
                resource,
                message,
                applicationAccessToken ?? Facebook.ApplicationAccessToken());

            using (var webClient = new WebClient())
            {
                webClient.UploadString(uri, string.Empty);
            }
        }

        // http://stackoverflow.com/questions/3385593/c-hmacsha256-problem-matching-facebook-signed-request-implementation
        // http://stackoverflow.com/questions/4575932/read-oauth2-0-signed-request-facebook-registration-c-mvc
        // Facebook C# SDK
        public static dynamic SignedRequest(string applicationSecret = null, string signedRequest = null)
        {
            var signed = signedRequest ?? HttpContext.Current.Request.Unvalidated("signed_request");

            if (signed.IsEmpty())
                return null;

            var parts = signed.Split('.');
            var signature = parts[0];
            var payload = parts[1];
            var hash = Hash(applicationSecret ?? ApplicationSecret, payload);

            if (signature.Decode() != hash.Decode())
                return null;

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload.Decode()));

            return Json.Decode(json);
        }

        private static string Hash(string key, string body)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
                return Convert.ToBase64String(hmac.Hash);
            }
        }

        private static string Decode(this string encoded)
        {
            encoded = encoded.Replace('+', '-').Replace('/', '_').Trim();
            int pad = encoded.Length % 4;
            if (pad > 0)
            {
                pad = 4 - pad;
            }
            encoded = encoded.PadRight(encoded.Length + pad, '=');
            return encoded;
        }

        public static bool HasLiked()
        {
            var signedRequest = SignedRequest();
            return signedRequest != null && signedRequest.page != null && signedRequest.page.liked == true;
        }

        public static bool IsPageAdmin()
        {
            var signedRequest = SignedRequest();
            return signedRequest != null && signedRequest.page != null && signedRequest.page.admin == true;
        }

        // http://www.colingodsey.com/facebook-api-fql-for-friends-with-app/
        public static List<string> AppFriends(string accessToken)
        {
            var result = new List<string>();
            var friends = Facebook.Request("fql?q=SELECT uid FROM user WHERE has_added_app=1 and uid IN (SELECT uid2 FROM friend WHERE uid1 = me())", accessToken);
            
            foreach (var friend in friends.data)
            {
                result.Add(friend.uid);
            }

            return result;
        }

        public static List<string> Friends(string accessToken)
        {
            var ids = new List<string>();
            GetList("me/friends", accessToken, ids);
            return ids;
        }

        private static void GetList(string uri, string accessToken, List<string> ids) {
            var list = Facebook.Request(uri, accessToken);

            foreach (var entry in list.data)
            {
                ids.Add(entry.id);
            }

            if (list.paging != null && list.paging.next != null)
            {
                GetList(list.paging.next, accessToken, ids);
            }
        }

        public static User Authenticated(string accessToken)
        {
            dynamic facebookUser = Facebook.Request("me", accessToken);
            string facebookId = facebookUser.id;
            string emailAddress = facebookUser.email;

            if ((Facebook.Scope.Contains("email") && !emailAddress.IsEmpty()) || facebookUser.first_name != null)
            {
                var identity = new Identity() 
                {
                    Provider = "Facebook",
                    Key = facebookId,
                    AccessToken = accessToken
                };

                string userName = emailAddress ?? identity.ToString();
                
                FormsAuthentication.SetAuthCookie(userName, false); // persistant cookie not required, as signed_request will re-login user

                var applicationModel = WebApp.GetService<IApplicationModel>();
                var user = applicationModel.Users.Where(FilterBy.UserName("facebook:" + facebookId)).FirstOrDefault()
                    ?? applicationModel.Users.Where(FilterBy.UserName(emailAddress)).FirstOrDefault();
                    
                if (user == null)
                {                       
                    string location = facebookUser.location == null ? string.Empty : facebookUser.location.name;
                    string birthday = facebookUser.birthday;

                    user = new User()
                    {
                        FirstName = facebookUser.first_name,
                        LastName = facebookUser.last_name,
                        EmailAddress = emailAddress,
                        Locale = facebookUser.locale,
                        Location = location,
                        Identity = identity,
                        DateOfBirth = birthday.AsDateTime()
                    };

                    applicationModel.Users.Add(user);
                }
                else
                {
                    user.Identity = identity;
                }

                applicationModel.SaveChanges();

                return user;
            }
            else
            {
                return null;
            }            
        }

        public enum EmbedType
        {
            Html,
            Iframe
        }

        public enum PictureSize {
            Square,
            Small,
            Normal,
            Large
        }

        public enum Connection
        {
            Accounts,
            Activities,
            Albums,
            AppRequests,
            Attending,
            Books,
            Checkins,
            Comments,
            Declined,
            Events,
            Family,
            Feed,
            FriendLists,
            Friends,
            Games,
            Groups,
            Home,
            Inbox,
            Interests,
            Likes,
            Links,
            Maybe,
            Movies,
            Music,
            Notes,
            Outbox,
            Payments,
            Permissions,
            Photos,
            Picture,
            Pokes,
            Posts,
            Statuses,
            Tagged,
            Television,
            Updates,
            Videos
        }

        public class Post
        {
            [Required]
            public string Message { get; set; }
            [Required]
            public string Link { get; set; }
            public string Picture { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Caption { get; set; }
            public List<Action> Actions { get; set; }
            public string Privacy { get; set; }
        }

        public class Action
        {
            public string Name { get; set; }
            public string Link { get; set; }
        }

        public class OpenGraph
        {
            public string Title { get; set; }
            public string Type { get; set; }
            public string Url { get; set; }
            public string Image { get; set; }
            public string Description { get; set; }
        }
    }
}