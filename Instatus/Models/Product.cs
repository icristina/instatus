using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instatus.Models
{
    public class Product
    {
        public int Id { get; set; }

        // content
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Video { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }

        // publishing
        public string Locale { get; set; }
        public State State { get; set; }
        public DateTime Created { get; set; }

        // commerce
        public double Price { get; set; }
        public string Currency { get; set; }

        // associations
        public virtual Campaign Campaign { get; set; }
        public int? CampaignId { get; set; }

        public Product()
        {
            Created = DateTime.UtcNow;
        }
    }
}
