using System;
using System.Threading.Tasks;
using Bot.Instagram.Model;
using Bot.Interface;

namespace User.Clients
{
    /// <summary>
    /// Client interface providing interface definition for bot.
    /// </summary>
    public abstract class Client : IClient
    {
        public System.Action onFindJobs; 
        public System.Action<int> onProcessed;
        public System.Action<int, int> onProcessUpdate;
        public System.Action<InstaLog> onLogWork;
        public System.Action<string, int> onLogMessage;

        public abstract Task<Schedule> FindJobs();
        public abstract Task<SimpleResult> LogMessage(string message, int type);
        public abstract Task<SimpleResult> LogWork(InstaLog jobLog);
        public abstract Task<SimpleResult> SetJobProcessed(int jobId);
        public abstract Task<SimpleResult> UpdateJobProcess(int jobId, int count);
    }
}
