using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(colorwebsitemobileService.Startup))]

namespace colorwebsitemobileService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}