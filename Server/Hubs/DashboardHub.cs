using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Blazor.Server.Hubs
{
    public class DashboardHub : Hub
    {
        public async Task DashboardDragAndDrop(string id, int row, int column)
        {
            await Clients.All.SendAsync("LayoutChange", id, row, column);
        }
    }
}
