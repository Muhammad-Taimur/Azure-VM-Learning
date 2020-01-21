using Microsoft.Owin;
using Owin;

namespace JobsAuctions
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}