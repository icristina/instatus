using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Instatus.Data;

namespace Instatus.Models
{
    public class Price : IEntity
    {
        public int Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double ListPrice { get; set; }
        public double RetailPrice { get; set; }
        public string Currency { get; set; }

        [IgnoreDataMember]
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }

        public virtual Organization Seller { get; set; }
        public int? SellerId { get; set; }

        public Price() { }

        public Price(double retailPrice)
        {
            RetailPrice = retailPrice;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", RetailPrice, Currency);
        }
    }
}