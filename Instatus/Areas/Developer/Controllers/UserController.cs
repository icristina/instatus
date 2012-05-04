using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Models;
using Instatus.Data;
using Instatus.Controllers;
using Instatus.Web;
using Instatus.Services;
using System.IO;
using Instatus;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Instatus.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Instatus.Areas.Developer.Controllers
{
    public class UserViewModel : BaseViewModel<User, IApplicationModel>
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }   
        
        [DisplayName("Email Address")]
        [Required]
        public string EmailAddress { get; set; }        
        
        [DisplayName("Password")]
        public string UnencryptedPassword { get; set; }

        public string Role { get; set; }

        public override void Save(User model)
        {
            base.Save(model);
            
            if (!UnencryptedPassword.IsEmpty())
                model.Password = UnencryptedPassword.ToEncrypted();  
        }
    }
    
    [Authorize(Roles = WebConstant.Role.Developer)]
    [Description("Users")]
    [AddParts(Scope = WebConstant.Scope.Admin)]
    public class UserController : ScaffoldController<UserViewModel, User, IApplicationModel, int>
    {

    }
}
