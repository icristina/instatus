using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Instatus.Web
{
    public class ChromeOnlyAttribute : UserAgentAttribute
    {
        public ChromeOnlyAttribute() : base("Chrome", 0, false) { }
    }
    
    public class UserAgentAttribute : ActionFilterAttribute
    {
        public bool Exclude { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                var browser = filterContext.HttpContext.Request.Browser;
                var version = filterContext.HttpContext.Request.Browser.MajorVersion;

                try
                {
                    if (browser.Browser.Match(Name) && version < Version && Exclude)
                    {
                        var requires = Exclude ? "Requires" : "Unavailable for";
                        throw new UserAgentException(string.Format("{0} {1} {2}", requires, Name, Version));
                    }
                }
                catch (Exception exception)
                {
                    filterContext.Result = new FatalErrorResult(filterContext, exception);
                }
            }
        }

        public UserAgentAttribute(string name, int version, bool exclude)
        {
            Name = name;
            Version = version;
            Exclude = exclude;            
        }
    }

    public class UserAgentException : HttpException
    {
        public UserAgentException(string message)
            : base(message)
        {

        }
    }
}