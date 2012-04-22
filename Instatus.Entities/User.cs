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
        public string Password { get; set; }
        public Credential Credential { get; set; }
        public bool Verified { get; set; }
        public bool Suspended { get; set; }
        public Gender? Gender { get; set; }
        public Role? Role { get; set; }
        public virtual ICollection<Page> Pages { get; set; }

        public User()
        {
            Credential = new Credential();
            CreatedTime = DateTime.UtcNow;
        }
    }

    [ComplexType]
    public class Credential
    {
        public Provider Provider { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string AccessToken { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    [Flags]
    public enum Role : byte
    {
        Member = 0,
        Author = 1,
        Editor = 2,
        Moderator = 4,
        Developer = 8,
        Administrator = 16
    }
}
