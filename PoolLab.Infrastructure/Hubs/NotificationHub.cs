using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(Guid storeId, string message)
        {
            Console.WriteLine($"Sending message: {message}");
            await Clients.Group(storeId.ToString()).SendAsync("ReceiveNotification", message);
        }

        public async Task JoinBranchGroup(string? storeId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, storeId);
            Console.WriteLine($"Connection {Context.ConnectionId} joined branch group {storeId}");
        }
    }
}
