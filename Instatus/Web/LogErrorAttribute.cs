using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instatus.Data;
using Instatus.Models;
using System.Threading.Tasks;
using System.Text;

namespace Instatus.Web
{
    public class LogErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            using (var db = WebApp.GetService<IDataContext>())
            {
                if (db != null)
                {
                    db.LogError(filterContext.Exception);
                    db.SaveChanges();
                }
            }
        }
    }
}