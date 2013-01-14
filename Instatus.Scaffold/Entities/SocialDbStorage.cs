using Instatus.Core;
using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Core.Extensions;
using System.Security.Principal;
using Instatus.Core.Impl;

namespace Instatus.Scaffold.Entities
{
    public class SocialDbStorage : ITaxonomy, IMembership, IKeyValueStorage<Document>, IAuditing, ILocalization
    {        
        private IEntityStorage entityStorage;
        private IEncryption encryption;
        private ICache cache;
        private IPreferences preferences;
        private IHosting hosting;
        
        // Private
        private User GetUserByUserName(string userName)
        {
            var normalizedUserName = userName.ToNormalizedLower();

            if (!ValidateUserName(normalizedUserName))
            {
                return null;
            }

            return entityStorage.Set<User>()
                .Where(u => u.EmailAddress == normalizedUserName)
                .FirstOrDefault();
        }

        private string GenerateVerificationToken(User user)
        {
            user.VerificationToken = Guid.NewGuid().ToString();

            return user.VerificationToken;
        }

        private User GetUserByVerificationToken(string userName, string verificationToken)
        {
            var normalizedUserName = userName.ToNormalizedLower();
            
            if (!ValidateVerificationToken(verificationToken) || !ValidateUserName(normalizedUserName))
            {
                return null;
            }

            return entityStorage.Set<User>()
                .Where(u => u.EmailAddress == normalizedUserName && u.VerificationToken == verificationToken)
                .FirstOrDefault();
        }

        private bool ValidateUserName(string userName)
        {
            return !string.IsNullOrWhiteSpace(userName) && userName.Contains('@');
        }

        private bool ValidatePassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password);
        }

        private bool ValidateVerificationToken(string verificationToken)
        {
            Guid guid;

            return Guid.TryParse(verificationToken, out guid);
        }

        private Post GetPostByLocaleAndSlug(string locale, string slug)
        {
            return entityStorage.Set<Post>()
                .Where(p => p.Locale == locale && p.Slug == slug)
                .FirstOrDefault();
        }

        // ITaxonomy
        public string[] GetTags()
        {
            return entityStorage.Set<Tag>()
                .Select(t => t.Name)
                .ToArray();
        }

        // IMembership
        public bool ValidateUser(string userName, string password)
        {
            var normalizedUserName = userName.ToNormalizedLower();
            
            if (!ValidatePassword(password) || !ValidateUserName(normalizedUserName))
            {
                return false;
            }
            
            var encryptedPassword = encryption.Encrypt(password);

            return entityStorage.Set<User>()
                .Any(u => u.EmailAddress == normalizedUserName && u.Password == encryptedPassword);
        }

        public bool ValidateExternalUser(string providerName, string providerUserId, IDictionary<string, object> data, out string userName)
        {
            if (providerName.Equals(WellKnown.Provider.Facebook, StringComparison.OrdinalIgnoreCase))
            {
                var userSet = entityStorage.Set<User>();
                var user = userSet.Where(u => u.FacebookId == providerUserId).FirstOrDefault();

                if (user == null)
                {
                    user = new User()
                    {
                        FacebookId = providerUserId,
                        FirstName = data.GetValue<string>("first_name"),
                        LastName = data.GetValue<string>("last_name"),
                        EmailAddress = data.GetValue<string>("email"),
                        Locale = data.GetValue<string>("locale")
                    };
                    userSet.Add(user);
                    entityStorage.SaveChanges();
                }

                userName = providerUserId;

                return true;
            }

            userName = null;

            return false;
        }

        public string[] GetRoles(string userName)
        {
            var normalizedUserName = userName.ToNormalizedLower();

            if (!ValidateUserName(normalizedUserName))
            {
                return null;
            }
            
            var role = entityStorage
                .Set<User>()
                .Where(u => u.EmailAddress == normalizedUserName)
                .Select(u => u.Role)
                .FirstOrDefault();
            
            if (role == Role.Administrator) 
            {
                return WellKnown.Role.All; 
            }
            else if (role != null)
            {
                return new string[]
                { 
                    role.ToString()
                };
            }
            else
            {
                return null;
            }
        }

        public string GenerateVerificationToken(string userName)
        {
            var user = GetUserByUserName(userName);
            var verificationToken = GenerateVerificationToken(user);

            entityStorage.SaveChanges();

            return verificationToken;
        }

        public bool ValidateVerificationToken(string userName, string verificationToken)
        {
            var user = GetUserByVerificationToken(userName, verificationToken);

            if (user == null)
            {
                return false;
            }

            user.IsVerified = true;

            GenerateVerificationToken(user);

            entityStorage.SaveChanges();

            return true;
        }

        public bool ChangePassword(string userName, string verificationToken, string newPassword)
        {
            var user = GetUserByVerificationToken(userName, verificationToken);

            if (user == null || !ValidatePassword(newPassword))
            {
                return false;
            }

            user.Password = encryption.Encrypt(newPassword);

            GenerateVerificationToken(user);

            entityStorage.SaveChanges();

            return true;
        }

        // IKeyValueStorage<Document>
        public Document Get(string partitionKey, string rowKey)
        {
            return entityStorage.Set<Post>()
                .Where(p => p.Locale == partitionKey && p.Slug == rowKey)
                .Select(p => new Document()
                {
                    Title = p.Name,
                    Description = p.Content
                })
                .FirstOrDefault();
        }

        public IEnumerable<KeyValue<Document>> Query(string partitionKey, Criteria criteria)
        {
            return entityStorage
                .Set<Post>()
                .Where(p => p.Locale == partitionKey)
                .Select(p => new KeyValue<Document>()
                {
                    Key = p.Slug,
                    Value = new Document()
                    {
                        Title = p.Name,
                        Description = p.Content
                    }
                })
                .ToList();
        }

        public void AddOrUpdate(string partitionKey, string rowKey, Document value)
        {
            var post = GetPostByLocaleAndSlug(partitionKey, rowKey);

            if (post == null)
            {
                post = new Post() 
                {
                    Locale = partitionKey,
                    Slug = rowKey                     
                };

                entityStorage.Set<Post>().Add(post);
            }

            post.Name = value.Title;
            post.Content = value.Description;

            entityStorage.SaveChanges();
        }

        public void Delete(string partitionKey, string rowKey)
        {
            var post = GetPostByLocaleAndSlug(partitionKey, rowKey);

            if (post != null)
            {
                entityStorage.Set<Post>().Delete(post.Id);
                entityStorage.SaveChanges();
            }
        }

        // IAuditing
        public void Log(IPrincipal principal, string category, string uri, IDictionary<string, string> properties)
        {
            var userName = principal.Identity.Name;
            var user = GetUserByUserName(userName);

            if (user == null) 
            {
                return;
            }

            var audit = new Audit()
            {
                User = user,
                Category = category,
                Uri = uri
            };

            audit.SetPayload(properties);

            entityStorage.Set<Audit>().Add(audit);
            entityStorage.SaveChanges();
        }

        // ILocalization
        private IDictionary<Tuple<string, string>, string> phrases;

        private IDictionary<Tuple<string, string>, string> Phrases
        {
            get 
            {
                return phrases ?? (phrases = cache.Get<IDictionary<Tuple<string, string>, string>>("phrases", () => 
                {
                    return entityStorage.Set<Phrase>()
                        .ToList()
                        .ToDictionary(p => new Tuple<string, string>(p.Locale, p.Key), p => p.Text);
                    }
                ));
            }
        }

        public string Phrase(string key)
        {
            return new InMemoryLocalization(preferences, hosting, Phrases)
                .Phrase(key);
        }

        public string Format(string key, params object[] values)
        {
            return new InMemoryLocalization(preferences, hosting, Phrases)
                .Format(key, values);
        }

        public SocialDbStorage(IEntityStorage entityStorage, IEncryption encryption, ICache cache, IPreferences preferences, IHosting hosting)
        {
            this.entityStorage = entityStorage;
            this.encryption = encryption;
            this.cache = cache;
            this.preferences = preferences;
            this.hosting = hosting;
        }
    }
}