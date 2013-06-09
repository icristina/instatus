using System;
using System.Collections.Generic;
using System.Linq;

namespace Instatus.Models
{
    public class User
    {
        public int Id { get; set; }
        
        // name
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // membership
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        // publishing
        public DateTime Created { get; set; }

        // geospatial
        public string Locale { get; set; }
        public Address Address { get; set; }

        // preferences
        public bool IsOptedIn { get; set; }

        // associations
        public virtual ICollection<Entry> Entries { get; set; }
        public virtual ICollection<Page> Posts { get; set; }

        public User()
        {
            Created = DateTime.UtcNow;
            Address = new Address();
            Role = Role.Reader;

            // initialize collections
            Entries = new List<Entry>();
            Posts = new List<Page>();
        }
    }
}