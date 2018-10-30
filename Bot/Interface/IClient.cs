using Bot.Instagram.Model;
using System.Threading.Tasks;

namespace Bot.Interface
{
    /// <summary>
    /// Client interface definition.
    /// </summary>
    public interface IClient
    {
        Task<Schedule> FindJobs();
        Task<SimpleResult> SetJobProcessed(int jobId);
        Task<SimpleResult> UpdateJobProcess(int jobId, int count);
        Task<SimpleResult> LogWork(InstaLog jobLog);
        Task<SimpleResult> LogMessage(string message, int type);
    }
}
