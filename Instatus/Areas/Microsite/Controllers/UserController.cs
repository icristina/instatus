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

namespace Instatus.Areas.Microsite.Controllers
{
    public class UserViewModel : BaseViewModel<User, BaseDataContext>
    {
        [DisplayName("Full Name")]
        [Required]
        public string FullName { get; set; }        
        
        [DisplayName("Email Address (Username)")]
        [Required]
        public string EmailAddress { get; set; }        
        
        [DisplayName("Password")]
        [Required]
        public string UnencryptedPassword { get; set; }

        [Column("Roles")]
        public MultiSelectList RoleList { get; set; }

        [ScaffoldColumn(false)]
        public int[] Roles { get; set; }

        public override void Load(User model)
        {
            Roles = model.Roles.IsEmpty() ? null : model.Roles.Select(t => t.Id).ToArray();
            base.Load(model);
        }

        public override void Save(User model)
        {
            if (!UnencryptedPassword.IsEmpty())
                model.Password = UnencryptedPassword.ToEncrypted();

            model.Roles = UpdateList<Role, int>(Context.Roles, model.Roles, Roles);
            base.Save(model);
        }

        public override void Databind()
        {
            RoleList = new MultiSelectList(Context.Roles.ToList(), "Id", "Name", Roles);
        }
    }
    
    [Authorize(Roles = "Administrator")]
    public class UserController : ScaffoldController<UserViewModel, User, BaseDataContext, int>
    {
        public override void ConfigureWebView(WebView<User> webView)
        {
            webView.Permissions = WebRole.Administrator.ToPermissions();
            base.ConfigureWebView(webView);
        }
    }
}
