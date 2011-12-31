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

namespace Instatus.Areas.Facebook
{
    public static class Facebook
    {
        static Facebook()
        {
            PubSub.Provider.Subscribe<ApplicationReset>(a =>
            {
                credential = null;
            });
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
            
            var client = new WebClient();

            try
            {
                return client.DownloadString(uri);
            }
            catch
            {
                return null;
            }
        }

        public static string Picture(object resource, PictureSize size)
        {
            return string.Format("https://graph.facebook.com/{0}/picture?type={1}", resource, size.ToString().ToLower());
        }

        public static string Profile(object resource)
        {
            return string.Format("https://www.facebook.com/{0}", resource);
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
            var client = new WebClient();
            var values = graph.ToNameValueCollection();
            
            client.UploadValues(uri, values);
        }

        public static List<WebEntry> Feed(object resource, string accessToken)
        {
            var response = Request(resource, accessToken, Connection.Feed);
            var feed = new List<WebEntry>();

            foreach (var entry in response.data)
            {
                feed.Add(new WebEntry()
                {
                    Picture = entry.picture,
                    Kind = entry.type,
                    Title = entry.name,
                    Description = entry.description,
                    Timestamp = DateTime.Parse(entry.created_time)
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
                    using (var db = BaseDataContext.Instance())
                    {
                        var facebook = WebProvider.Facebook.ToString();
                        var environment = ConfigurationManager.AppSettings.Value<string>("Environment");

                        credential = db.Pages.OfType<Application>().First()
                                            .Credentials
                                            .Where(c => c.Provider == facebook && c.Environment == environment)
                                            .FirstOrDefault();
                    }
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

        public static string ApplicationName
        {
            get
            {
                return Credential.Name;
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
                return Credential.Uri;
            }
        }

        public static string CanvasUrl
        {
            get
            {
                return string.Format("https://apps.facebook.com/{0}", Credential.Name);
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

            return new WebClient().DownloadString(uri).SubstringAfter("access_token=");
        }

        public static void AppRequest(object resource, string message, string applicationAccessToken = null)
        {
            var uri = string.Format("https://graph.facebook.com/{0}/apprequests?message='{1}'&data=''&{2}&method=post",
                resource,
                message,
                applicationAccessToken ?? Facebook.ApplicationAccessToken());

            new WebClient().UploadString(uri, string.Empty);
        }

        // http://stackoverflow.com/questions/3385593/c-hmacsha256-problem-matching-facebook-signed-request-implementation
        // http://stackoverflow.com/questions/4575932/read-oauth2-0-signed-request-facebook-registration-c-mvc
        // Facebook C# SDK
        public static dynamic SignedRequest(string applicationSecret = null, string signedRequest = null)
        {
            var signed = signedRequest ?? HttpContext.Current.Request.Params["signed_request"];

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
                var credential = new Credential(WebProvider.Facebook, facebookId, accessToken);

                string userName = emailAddress ?? credential.ToUrn();
                
                FormsAuthentication.SetAuthCookie(userName, false); // persistant cookie not required, as signed_request will re-login user

                using (var db = BaseDataContext.Instance())
                {
                    var user = db.GetUser(WebProvider.Facebook, facebookId) ?? db.GetUser(emailAddress);
                    
                    if (user == null)
                    {                       
                        string location = facebookUser.location == null ? string.Empty : facebookUser.location.name;
                        string birthday = facebookUser.birthday;

                        user = new User()
                        {
                            FullName = facebookUser.name,
                            Name = new Name()
                            {
                                GivenName = facebookUser.first_name,
                                FamilyName = facebookUser.last_name
                            },
                            EmailAddress = emailAddress,
                            Locale = facebookUser.locale,
                            Location = location,
                            Credentials = new List<Credential>() {
                                credential
                            },
                            DateOfBirth = birthday.AsDateTime()
                        };

                        db.Users.Add(user);
                    }
                    else
                    {
                        if (user.Credentials.IsEmpty())
                            user.Credentials = new List<Credential>();

                        var existingCredential = user.Credentials
                                .Where(c => c.Provider == WebProvider.Facebook.ToString())
                                .FirstOrDefault();

                        if (existingCredential != null)
                            db.Sources.Remove(existingCredential);

                        user.Credentials.Add(credential);
                    }

                    db.SaveChanges();

                    return user;
                }
            }
            else
            {
                return null;
            }            
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