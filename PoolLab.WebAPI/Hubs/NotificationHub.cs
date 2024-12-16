using Microsoft.AspNetCore.SignalR;

namespace PoolLab.WebAPI.Hubs
{
    public class NotificationHub:Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
