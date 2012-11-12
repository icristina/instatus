using Instatus.Core;
using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Instatus.Scaffold.Entities
{
    public class Place : IPage, ICreated, IPayload
    {
        public int Id { get; set; }        
        public DbGeography Point { get; set; }
        public Address Address { get; set; }       
        
        // IPage
        public string Slug { get; set; }
        public string Locale { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public string Category { get; set; }

        // ICreated
        public DateTime Created { get; set; }

        // IPayload
        public string Data { get; set; }    

        public Place()
        {
            Created = DateTime.UtcNow;
            Address = new Address();
        }
    }
}