using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProgressBarWithSignalR.Hubs
{
    public class ProgressHub : Hub
    {
        private static int _connectionsCount = 0;

        public override Task OnConnectedAsync()
        {
            Clients.All.SendAsync("updateCount", Interlocked.Increment(ref _connectionsCount));
            Clients.All.SendAsync("connected", Context.ConnectionId);

            base.OnConnectedAsync();

            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.All.SendAsync("updateCount", Interlocked.Decrement(ref _connectionsCount));

            base.OnDisconnectedAsync(exception);

            return Task.CompletedTask;
        }
    }
}
