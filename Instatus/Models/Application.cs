using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Instatus.Models
{
    public class Application : Page
    {
        public virtual ICollection<Domain> Domains { get; set; }
        public virtual ICollection<Page> Content { get; set; }
        public virtual ICollection<Credential> Credentials { get; set; }
        public virtual ICollection<Taxonomy> Taxonomies { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<Phrase> Phrases { get; set; }
    }
}