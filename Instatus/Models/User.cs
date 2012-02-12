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
using System.Web.Routing;
using System.Web.Mvc;
using Instatus.Areas.Auth;
using Instatus.Services;
using System.Net.Mail;

namespace Instatus.Models
{
    public class User
    {
        public int Id { get; set; }

        private string emailAddress;

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

        public Name Name { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; } // ie. short name or alias

        public string Locale { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedTime { get; set; }

        public string Status { get; set; }
        public string Permission { get; set; }

        public virtual Source Source { get; set; }
        public int? SourceId { get; set; }

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
            return FullName ?? Name.ToString();
        }

        public User()
        {
            Password = Generator.Password();
            CreatedTime = DateTime.UtcNow;
            Name = new Name();
            Extensions = new ExpandoObject();
            Status = WebStatus.PendingApproval.ToString(); // PendingApproval, not auto Approved
            Permission = WebPermission.Public.ToString();
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

        public string GetVerificationToken()
        {
            return Password.Substring(0, 10);
        }

        public bool ValidateVerificationToken(string token)
        {
            if (GetVerificationToken().Match(token))
            {
                if (Status.AsEnum<WebStatus>() == WebStatus.PendingApproval)
                {
                    Status = WebStatus.Approved.ToString();
                }
                
                return true;
            }
            
            return false;
        }

        public string GenerateVerificationUri(UrlHelper urlHelper)
        {
            return urlHelper.Absolute(WebRoute.AccountVerification, new { id = Id, token = GetVerificationToken() });
        }

        public void GenerateVerificationEmail(string subject = null, string senderEmailAddress = null, string senderDisplayName = null)
        {
            if (Status.AsEnum<WebStatus>() == WebStatus.PendingApproval && !EmailAddress.IsEmpty())
            {
                var templateService = DependencyResolver.Current.GetService<ITemplateService>();
                var from = senderEmailAddress ?? string.Format("noreply@{0}", WebPath.BaseUri.Host);
                var body = templateService.Process("Notification", this);
                var mailMessage = new MailMessage(new MailAddress(from, senderDisplayName), new MailAddress(EmailAddress))
                {
                    Subject = subject ?? body.FindHtmlElement("title"),
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.Send();
            }
        }
    }
}