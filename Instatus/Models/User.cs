using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Web.Security;
using Instatus.Data;
using System.Dynamic;
using Instatus.Web;

namespace Instatus.Models
{
    public class User
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }

        [IgnoreDataMember]
        public string Password { get; set; }

        public Name Name { get; set; }
        public string FullName { get; set; }

        public string Locale { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedTime { get; set; }

        public string Status { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Credential> Credentials { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public virtual ICollection<User> Friends { get; set; }
        public virtual ICollection<User> Relationships { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<List> Lists { get; set; }

        public virtual ICollection<Preference> Preferences { get; set; }

        [NotMapped]
        public dynamic Extensions { get; set; }

        public override string ToString()
        {
            return FullName;
        }

        public User()
        {
            Password = Generator.Password().ToEncrypted();
            CreatedTime = DateTime.Now;
            Name = new Name();
            Extensions = new ExpandoObject();
            Status = WebStatus.Approved.ToString();
        }

        public User(string fullName)
            : this()
        {
            FullName = fullName;
        }

        public static User From(object entry)
        {
            return entry is IUserGeneratedContent ? ((IUserGeneratedContent)entry).User : (User)entry;
        }
    }
}