using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Blazor.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string senderName, string receiverName,
            string msgTitle, string msgBody)
        {
            await Clients.All.SendAsync("ReceiveMessage", senderName, receiverName, msgTitle, msgBody);
        }
    }
}
