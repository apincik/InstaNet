using Bot.Exception;
using Bot.Instagram.Model;
using System.Threading.Tasks;
using BotStats = Bot.Instagram.Model.Stats;
using LogEnum = Bot.Instagram.Model.Enum.Log;
using BotLog = Bot.Instagram.Model.Log;

namespace User.Bots
{
    /// <summary>
    /// Bot for reading my profile stats.
    /// </summary>
    /// <remarks>
    /// Value returned as plain json string in message property.
    /// </remarks>
    public class Stats : Bot
    {
        public override async Task Run(Job job)
        {
            BotStats stats = await _manager.GetProfileStats(job.Instagram.Username);
            BotLog log = new BotLog()
            {
                ScheduleId = job.Id,
                Username = job.Instagram.Username,
                Link = "",
                Message = stats.ToJson(),
                Type = (int) LogEnum.TYPE_STATS,
            };

            InstaLog logData = new InstaLog();
            logData.Data.Add(log);

            await _client.LogWork(logData);
            await _client.UpdateJobProcess(job.Id, job.Limit);
            await _client.SetJobProcessed(job.Id);
        }
    }
}
