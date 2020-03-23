using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BlazorConfTool.Server.Hubs
{
    public class ConferencesHub : Hub
    {
        public async Task BroadcastNewConferenceAdded()
        {
            await Clients.All.SendAsync("NewConferenceAdded");
        }
    }
}
