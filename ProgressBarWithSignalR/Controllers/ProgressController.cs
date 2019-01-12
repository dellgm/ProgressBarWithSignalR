using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProgressBarWithSignalR.Hubs;
using Tasks;

namespace ProgressBarWithSignalR.Controllers
{
    public class MyProgress<T> : IProgress<T>
    {
        public event ProgressChangedEventHandler<T> ProgressChanged;

        public void Report(T value)
        {
            ProgressChanged?.Invoke(this, value);
        }
    }

    public delegate void ProgressChangedEventHandler<T>(object sender, T progress);

    public class ProgressController : Controller
    {
        private readonly IHubContext<ProgressHub> _progressHubContext;

        public ProgressController(IHubContext<ProgressHub> progressHubContext)
        {
            _progressHubContext = progressHubContext;
        }

        public void Lengthy([Bind(Prefix = "id")] string connId)
        {
            var steps = new Random().Next(3, 99);
            var increase = (int)100 / steps;

            // NOTIFY START
            _progressHubContext.Clients.Client(connId).SendAsync("initProgressBar");

            var total = 0;

            for (var i = 0; i < steps; i++)
            {
                Thread.Sleep(500);
                total += increase;

                // PROGRESS
                _progressHubContext.Clients.Client(connId).SendAsync("updateProgressBar", total);
            }

            // NOTIFY END
            _progressHubContext.Clients.Client(connId).SendAsync("clearProgressBar");
        }


        public string Conn { get; set; }
        public async Task Taskytis([Bind(Prefix = "id")] string connId)
        {
            Conn = connId;
            var j = new Job();

            await _progressHubContext.Clients.Client(connId).SendAsync("initProgressBar");

            var progress = new MyProgress<ReportModel>();
            progress.ProgressChanged += Progress_ProgressChanged;

            await j.Get(progress);
        }

        private void Progress_ProgressChanged(object sender, ReportModel e)
        {
            _progressHubContext.Clients.Client(Conn).SendAsync("updateProgressBar", e.PercentCompleted);

            if(e.PercentCompleted == 100)
            {
                _progressHubContext.Clients.Client(Conn).SendAsync("clearProgressBar");
            }
        }
    }
}