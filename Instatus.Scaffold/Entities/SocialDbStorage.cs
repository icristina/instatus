using Instatus.Core;
using Instatus.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instatus.Core.Extensions;

namespace Instatus.Scaffold.Entities
{
    public class SocialDbStorage : ITaxonomy, IMembership, IKeyValueStorage<Document>
    {        
        private IEntityStorage entityStorage;
        private IPreferences preferences;
        private IEncryption encryption;
        
        // ITaxonomy
        public string[] GetTags()
        {
            return entityStorage.Set<Tag>().Select(t => t.Name).ToArray();
        }

        // IMembership
        public bool ValidateUser(string userName, string password)
        {
            var encryptedPassword = encryption.Encrypt(password);

            return entityStorage.Set<User>().Any(u => u.EmailAddress == userName && u.Password == encryptedPassword);
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
            return new string[] { 
                entityStorage.Set<User>().Where(u => u.EmailAddress == userName).Select(u => u.Role).FirstOrDefault().ToString()
            };
        }

        public string GenerateVerificationToken(string userName)
        {
            var user = entityStorage.Set<User>().Where(u => u.EmailAddress == userName).FirstOrDefault();
            var verificationToken = Guid.NewGuid().ToString();

            user.Token = verificationToken;
            
            entityStorage.SaveChanges();

            return verificationToken;
        }

        public bool ValidateVerificationToken(string userName, string verificationToken)
        {
            var user = entityStorage.Set<User>().Where(u => u.EmailAddress == userName && u.Token == verificationToken).FirstOrDefault();

            if (user == null)
                return false;

            user.IsVerified = true;
            
            entityStorage.SaveChanges();

            return true;
        }

        public void ChangePassword(string userName, string verificationToken, string newPassword)
        {
            var user = entityStorage.Set<User>().Where(u => u.EmailAddress == userName && u.Token == verificationToken).FirstOrDefault();

            user.Password = encryption.Encrypt(newPassword);

            entityStorage.SaveChanges();
        }

        // IKeyValueStorage<Document>
        public Document Get(string key)
        {
            return entityStorage.Set<Post>().Where(p => p.Locale == preferences.Locale && p.Slug == key)
                .Select(p => new Document()
                {
                    Title = p.Name,
                    Description = p.Content
                })
                .FirstOrDefault();
        }

        public IEnumerable<KeyValue<Document>> Query(Criteria criteria)
        {
            return entityStorage
                .Set<Post>()
                .Where(p => p.Locale == preferences.Locale)
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

        public void AddOrUpdate(string key, Document value)
        {
            var post = entityStorage.Set<Post>().Where(p => p.Locale == preferences.Locale && p.Slug == key).FirstOrDefault();

            if (post == null)
            {
                post = new Post() 
                {
                    Locale = preferences.Locale,
                    Slug = key                     
                };
                entityStorage.Set<Post>().Add(post);
            }

            post.Name = value.Title;
            post.Content = value.Description;

            entityStorage.SaveChanges();
        }

        public void Delete(string key)
        {
            var postSet = entityStorage.Set<Post>();
            var post = postSet.Where(p => p.Locale == preferences.Locale && p.Slug == key).FirstOrDefault();

            if (post != null)
            {
                postSet.Delete(post.Id);
                entityStorage.SaveChanges();
            }
        }

        public SocialDbStorage(IEntityStorage entityStorage, IPreferences preferences, IEncryption encryption)
        {
            this.entityStorage = entityStorage;
            this.preferences = preferences;
            this.encryption = encryption;
        }
    }
}