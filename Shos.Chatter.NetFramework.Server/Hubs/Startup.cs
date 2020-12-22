using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Shos.Chatter.NetFramework.Server.Hubs.Startup))]

namespace Shos.Chatter.NetFramework.Server.Hubs
{
    public class Startup
    {
        public void Configuration(IAppBuilder app) => app.MapSignalR();
    }
}
