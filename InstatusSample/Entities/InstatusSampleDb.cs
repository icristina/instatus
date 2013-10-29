using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InstatusSample.Entities
{
    public class InstatusSampleDb : DbContext
    {
        public IDbSet<User> Users { get; set; }

        public InstatusSampleDb()
            : base("InstatusSample")
        {

        }
    }
}