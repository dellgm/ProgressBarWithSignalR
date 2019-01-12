using System.Threading.Tasks;

namespace ProgressBarWithSignalR.Hubs
{
    /// <summary>
    /// Client side method signatures
    /// </summary>
    public interface IProgressHubClient
    {
        Task InitProgressBar();
        Task UpdateProgressBar(int total);
        Task ClearProgressBar();
        Task UpdateCount(int count);
        Task Connected(string connectionId);
    }
}
