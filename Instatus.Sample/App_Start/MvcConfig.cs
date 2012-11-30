using Instatus.Integration.Mvc;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Instatus.Sample
{
    public class MvcConfig
    {
        public static void RegisterDisplayModes()
        {
            BaseMvcConfig.RegisterDisplayModes(DisplayModeProvider.Instance);
            BaseMvcConfig.RegisterEmbeddedResourceVirtualPathProvider<Instatus.Scaffold.Models.Blog>();
        }
    }
}