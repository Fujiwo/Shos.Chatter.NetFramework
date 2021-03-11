using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Shos.Chatter.Server.Hubs.Startup))]

namespace Shos.Chatter.Server.Hubs
{
    public class Startup
    {
        public void Configuration(IAppBuilder app) => app.MapSignalR();
    }
}
