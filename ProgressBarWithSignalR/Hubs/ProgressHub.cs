using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProgressBarWithSignalR.Hubs
{
    public class ProgressHub : Hub<IProgressHubClient>
    {
        private int _connectionsCount = 0;

        public override Task OnConnectedAsync()
        {
            Clients.All.UpdateCount(Interlocked.Increment(ref _connectionsCount));
            Clients.All.Connected(Context.ConnectionId);

            base.OnConnectedAsync();

            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.All.UpdateCount(Interlocked.Decrement(ref _connectionsCount));

            base.OnDisconnectedAsync(exception);

            return Task.CompletedTask;
        }

        public Task InitProgress()
        {
            return Clients.Caller.InitProgressBar();
        }

        public Task UpdateProgress(int progress)
        {
            return Clients.Caller.UpdateProgressBar(progress);
        }

        public Task ClearProgress()
        {
            return Clients.Caller.ClearProgressBar();
        }
    }
}
