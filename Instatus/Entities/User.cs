using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Instatus.Data;
using Instatus.Web;

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

        private string emailAddress;

        [Required]
        [RegularExpression(WebConstant.RegularExpression.EmailAddress)]
        public string EmailAddress
        {
            get
            {
                return emailAddress;
            }
            set
            {
                if (value.IsEmpty())
                {
                    emailAddress = null;
                }
                else
                {
                    emailAddress = value.ToLower(); // normalize email address, ensure lowercase
                }
            }
        }

        private string password;

        [IgnoreDataMember]
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (value.IsEmpty())
                {
                    password = Generator.Password().ToEncrypted();
                }
                else if (!value.IsEncrypted())
                {
                    password = value.ToEncrypted();
                }
                else
                {
                    password = value;
                }
            }
        }

        public DateTime? DateOfBirth { get; set; }
        
        public string Location { get; set; }
        public Identity Identity { get; set; }
        public Segment Segment { get; set; }
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
            Segment = new Segment();
            CreatedTime = DateTime.UtcNow;
        }
    }
}
