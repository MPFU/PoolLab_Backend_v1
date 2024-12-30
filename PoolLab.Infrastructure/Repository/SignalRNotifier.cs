using Microsoft.AspNetCore.SignalR;
using PoolLab.Core.Interface;
using PoolLab.Infrastructure.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Repository
{
    public class SignalRNotifier : ISignalRNotifier
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotifier(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyTableActivationAsync(Guid storeId, string message)
        {
            await _hubContext.Clients.Group(storeId.ToString()).SendAsync("ReceiveNotification", message);
        }
    }
}
