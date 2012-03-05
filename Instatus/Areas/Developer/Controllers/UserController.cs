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

namespace Instatus.Areas.Developer.Controllers
{
    public class UserViewModel : BaseViewModel<User, IApplicationContext>
    {
        [DisplayName("Full Name")]
        [Required]
        public string FullName { get; set; }        
        
        [DisplayName("Email Address")]
        [Required]
        public string EmailAddress { get; set; }        
        
        [DisplayName("Password")]
        public string UnencryptedPassword { get; set; }

        [Column("Roles")]
        [Display(Name = "Roles")]
        public MultiSelectList RoleList { get; set; }

        [ScaffoldColumn(false)]
        public int[] Roles { get; set; }

        public override void Load(User model)
        {
            base.Load(model);
            
            Roles = LoadMultiAssociation(model.Roles);            
        }

        public override void Save(User model)
        {
            base.Save(model);
            
            if (!UnencryptedPassword.IsEmpty())
                model.Password = UnencryptedPassword.ToEncrypted();

            model.Roles = SaveMultiAssociation<Role>(Context.Roles, model.Roles, Roles);            
        }

        public override void Databind()
        {
            RoleList = new MultiSelectList(Context.Roles.ToList(), "Id", "Name", Roles);
        }
    }
    
    [Authorize(Roles = "Administrator")]
    [Description("Users")]
    public class UserController : ScaffoldController<UserViewModel, User, IApplicationContext, int>
    {

    }
}
