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

            user.VerificationToken = verificationToken;
            
            entityStorage.SaveChanges();

            return verificationToken;
        }

        public bool ValidateVerificationToken(string userName, string verificationToken)
        {
            var user = entityStorage.Set<User>().Where(u => u.EmailAddress == userName && u.VerificationToken == verificationToken).FirstOrDefault();

            if (user == null)
                return false;

            user.IsVerified = true;
            
            entityStorage.SaveChanges();

            return true;
        }

        public void ChangePassword(string userName, string verificationToken, string newPassword)
        {
            var user = entityStorage.Set<User>().Where(u => u.EmailAddress == userName && u.VerificationToken == verificationToken).FirstOrDefault();

            user.Password = encryption.Encrypt(newPassword);

            entityStorage.SaveChanges();
        }

        // IKeyValueStorage<Document>
        public Document Get(string partitionKey, string rowKey)
        {
            return entityStorage.Set<Post>().Where(p => p.Locale == partitionKey && p.Slug == rowKey)
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
            var post = entityStorage.Set<Post>().Where(p => p.Locale == partitionKey && p.Slug == rowKey).FirstOrDefault();

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
            var postSet = entityStorage.Set<Post>();
            var post = postSet.Where(p => p.Locale == partitionKey && p.Slug == rowKey).FirstOrDefault();

            if (post != null)
            {
                postSet.Delete(post.Id);
                entityStorage.SaveChanges();
            }
        }

        public SocialDbStorage(IEntityStorage entityStorage, IEncryption encryption)
        {
            this.entityStorage = entityStorage;
            this.encryption = encryption;
        }
    }
}