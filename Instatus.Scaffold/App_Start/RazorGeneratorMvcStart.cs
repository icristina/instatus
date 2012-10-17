using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using RazorGenerator.Mvc;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Instatus.Scaffold.App_Start.RazorGeneratorMvcStart), "Start")]

namespace Instatus.Scaffold.App_Start {
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