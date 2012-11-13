using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using RazorGenerator.Mvc;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Instatus.Scaffold.RazorGeneratorMvcStart), "Start")]

namespace Instatus.Scaffold {
    public static class RazorGeneratorMvcStart {
        public static void Start() {
            var engine = new PrecompiledMvcEngine(typeof(RazorGeneratorMvcStart).Assembly) 
            {
                UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal
            };

            ViewEngines.Engines.Add(engine); // normal cshtml view engine takes precedence, added to end of list

            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
        }
    }
}