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
        private readonly IHubContext<ProgressHub, IProgressHubClient> _progressHubContext;

        public ProgressController(IHubContext<ProgressHub, IProgressHubClient> progressHubContext)
        {
            _progressHubContext = progressHubContext;
        }

        public void Lengthy([Bind(Prefix = "id")] string connId)
        {
            var steps = new Random().Next(3, 99);
            var increase = (int)100 / steps;

            _progressHubContext.Clients.Client(connId).InitProgressBar();

            var total = 0;

            for (var i = 0; i < steps; i++)
            {
                Thread.Sleep(500);
                total += increase;

                _progressHubContext.Clients.Client(connId).UpdateProgressBar(total);
            }

            _progressHubContext.Clients.Client(connId).ClearProgressBar();
        }

        public string Conn { get; set; }

        public async Task Taskytis([Bind(Prefix = "id")] string connId)
        {
            Conn = connId;
            var j = new Job();

            await _progressHubContext.Clients.Client(Conn).InitProgressBar();

            var progress = new MyProgress<ReportModel>();
            progress.ProgressChanged += Progress_ProgressChanged;

            await j.Get(progress);
        }

        private void Progress_ProgressChanged(object sender, ReportModel e)
        {
            _progressHubContext.Clients.Client(Conn).UpdateProgressBar(e.PercentCompleted);

            if (e.PercentCompleted == 100)
            {
                _progressHubContext.Clients.Client(Conn).ClearProgressBar();
            }
        }
    }
}