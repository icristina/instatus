using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Instatus.Models;
using Instatus.Web;
using Instatus.Data;

namespace Instatus.Entities
{   
    public class Page : IEntity, IContentItem, IUserGeneratedContent, INavigableContent, INamed
    {
        public int Id { get; set; }
        
        public int Locale { get; set; }
        
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime PublishedTime { get; set; }

        [Required]
        [RegularExpression(WebConstant.RegularExpression.Alias)]
        public string Alias { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Picture { get; set; }
        public Source Source { get; set; }
        public Card Card { get; set; }
        public Location Location { get; set; }
        public Schedule Schedule { get; set; }
        public Availability Availability { get; set; }
        public User User { get; set; }
        public Application Application { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public virtual ICollection<Association> Parents { get; set; }
        public virtual ICollection<Association> Children { get; set; }
#if NET45
        public Kind Kind { get; set; }
        public Published Published { get; set; }
        public Privacy Privacy { get; set; }
#else
        public string Kind { get; set; }
        public string Published { get; set; }
        public string Privacy { get; set; }
#endif

        [NotMapped]
        [ScaffoldColumn(false)]
        public Document Document
        {
            get
            {
                return Fields["Document"] as Document;
            }
            set
            {
                Fields["Document"] = value;
            }
        }

        private Dictionary<string, object> fields;

        [NotMapped]
        [IgnoreDataMember]
        [ScaffoldColumn(false)]
        public Dictionary<string, object> Fields
        {
            get
            {
                if (fields == null)
                {
                    fields = new Dictionary<string, object>()
                    {
                        { "Document", new Document() }
                    };
                }

                return fields;
            }
            set
            {
                fields = value;
            }
        }

        [IgnoreDataMember]
        [ScaffoldColumn(false)]
        public byte[] Payload
        {
            get
            {
                return Fields.AllEmpty() ? null : Fields.Serialize(Startup.KnownTypes);
            }
            set
            {
                Fields = value.Deserialize<Dictionary<string, object>>(Startup.KnownTypes);
            }
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }

        public Page(Kind kind) : this()
        {
            Kind = kind.ToString();
        }

        public Page()
        {
            Source = new Source();
            Card = new Card();
            Location = new Location();
            Schedule = new Schedule();
            Availability = new Availability();
            CreatedTime = DateTime.UtcNow;
            UpdatedTime = CreatedTime;
            PublishedTime = CreatedTime;
        }
    }
}
