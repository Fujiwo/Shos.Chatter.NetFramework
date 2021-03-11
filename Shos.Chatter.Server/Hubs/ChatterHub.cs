using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Shos.Chatter.Server.Hubs
{
    [HubName("chatter")]
    public class ChatterHub : Hub
    {
        public void UpdateUsers() => Clients.All.UpdateUsers();
        public void UpdateChats() => Clients.All.UpdateChats();
    }
}