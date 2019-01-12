using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    public class ReportModel
    {
        public int PercentCompleted { get; set; }
    }

    public class Job
    {
        public async Task Get(IProgress<ReportModel> progress)
        {
            var list = new List<int>();
            list.Add(1000);
            list.Add(1000);
            list.Add(1000);
            list.Add(1000);
            list.Add(1000);
            list.Add(1000);
            list.Add(1000);
            list.Add(1000);
            list.Add(1000);
            list.Add(1000);

            var report = new ReportModel();

            int count = 0;
            await Task.Run(() =>
            {
                //Parallel.For(0, 10, (depCode) =>
                //{
                //    Thread.Sleep(1000 * 5);
                //    count = count + 10;
                //    report.PercentCompleted = count;
                //    progress?.Report(report);
                //});

                //Parallel.ForEach(list, (depCode) =>
                //{
                //    Thread.Sleep(depCode * 5);
                //    count = count + 10;
                //    report.PercentCompleted = count;
                //    progress?.Report(report);
                //});

                foreach (var item in list)
                {
                    Thread.Sleep(item * 5);
                    count = count + 10;
                    report.PercentCompleted = count;
                    progress?.Report(report);
                }

            });
        }
    }
}
