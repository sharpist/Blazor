using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Blazor.Server.Hubs
{
    public class EmployeeHub : Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("ReceiveMessage");
        }
    }
}
