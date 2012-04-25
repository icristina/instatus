using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Instatus.Entities
{
    public class User
    {
        public int Id { get; set; }
        public int Locale { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Password { get; set; }
        public string Location { get; set; }
        public Identity Identity { get; set; }
        public bool Verified { get; set; }
        public bool Suspended { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public Application Application { get; set; }
        public string Role { get; set; }
#if NET45
        public Gender? Gender { get; set; }
#else
        public string Gender { get; set; }

#endif

        public User()
        {
            Identity = new Identity();
            CreatedTime = DateTime.UtcNow;
        }
    }

    [ComplexType]
    public class Identity
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string AccessToken { get; set; }
#if NET45
        public Provider Provider { get; set; }
#else
        public string Provider { get; set; }
#endif

        public string ToUrn()
        {
            return string.Format("urn:{0}:{1}", Provider.ToLower(), UserId.ToLower());
        }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
