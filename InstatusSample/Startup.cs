using Instatus.Server;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InstatusSample.Startup))]
namespace InstatusSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AuthConfig.RegisterGoogleConfiguration(app);
        }
    }
}
