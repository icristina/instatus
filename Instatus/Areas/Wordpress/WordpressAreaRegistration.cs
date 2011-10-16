using System.Web.Mvc;

namespace Instatus.Areas.Wordpress
{
    public class WordpressAreaRegistration : AreaRegistration
    {       
        public override string AreaName
        {
            get
            {
                return "Wordpress";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
  
        }
    }
}
