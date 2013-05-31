using System;
using System.Collections.Generic;
using System.Linq;

namespace Instatus.Models
{
    public class User
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Locale { get; set; }
        public Role Role { get; set; }
        public Address Address { get; set; }
        public DateTime Created { get; set; }
        public string Password { get; set; }

        // preferences
        public bool IsOptedIn { get; set; }

        // associations
        public virtual ICollection<Entry> Entries { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

        public User()
        {
            Created = DateTime.UtcNow;
            Address = new Address();
            Role = Role.Member;

            // initialize collections
            Entries = new List<Entry>();
            Posts = new List<Post>();
        }
    }
}