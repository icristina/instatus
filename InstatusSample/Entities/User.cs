using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstatusSample.Entities
{
    public class User : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public User()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}