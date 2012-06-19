﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Instatus.Data;

namespace Instatus.Entities
{
    public class User
    {
        public int Id { get; set; }
        public int Locale { get; set; }
        public DateTime CreatedTime { get; set; }
        
        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Picture { get; set; }

        private string emailAddress;

        [IgnoreDataMember]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName).Trim();
            }
        }

        [RegularExpression(WebConstant.RegularExpression.EmailAddress)]
        public string EmailAddress
        {
            get
            {
                return emailAddress;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
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
                if (string.IsNullOrWhiteSpace(value))
                {
                    password = Guid.NewGuid().ToString().ToEncrypted();                    
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

        public string FacebookAccount { get; set; }
        public string TwitterAccount { get; set; }
        
        public bool Verified { get; set; }
        public bool Suspended { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public Application Application { get; set; }

        public string Role { get; set; }
        public string Gender { get; set; }

        public void SetLocale(string locale)
        {
            var cultureInfo = new CultureInfo(locale.Replace('_', '-'));
            Locale = cultureInfo.LCID;
        }

        public override string ToString()
        {
            return FullName;
        }

        public User()
        {
            Identity = new Identity();
            Segment = new Segment();
            CreatedTime = DateTime.UtcNow;
        }
    }
}
