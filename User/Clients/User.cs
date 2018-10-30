using Bot.Instagram.Model;
using Bot.Interface;
using User.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotEnum = Bot.Instagram.Model.Enum.Job;

namespace User.Clients
{
    /// <summary>
    /// Local User client for passing jobs to bot manager.
    /// </summary>
    public class User : Client
    {
        private Schedule _schedule;

        public User()
        {
            _schedule = new Schedule();
        }

        public void AddJobBuilder(Builder jobBuilder)
        {
            _schedule.Jobs.Add(jobBuilder.Build());
        }

        public override async Task<Schedule> FindJobs()
        {
            onFindJobs?.Invoke();
            return _schedule;
        }

        public override async Task<SimpleResult> SetJobProcessed(int jobId)
        {
            onProcessed?.Invoke(jobId);
            return new SimpleResult() { Result = "success" };
        }

        public override async Task<SimpleResult> UpdateJobProcess(int jobId, int count)
        {
            onProcessUpdate?.Invoke(jobId, count);
            return new SimpleResult() { Result = "success" };
        }

        public override async Task<SimpleResult> LogWork(InstaLog jobLog)
        {
            onLogWork?.Invoke(jobLog);
            return new SimpleResult() { Result = "success" };
        }

        public override async Task<SimpleResult> LogMessage(string message, int type)
        {
            onLogMessage?.Invoke(message, type);
            return new SimpleResult() { Result = "success" };
        }
    }
}
