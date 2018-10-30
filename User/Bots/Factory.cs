using System.Collections.Generic;
using JobEnum = Bot.Instagram.Model.Enum.Job;

namespace User.Bots
{
    /// <summary>
    /// Map factory for storing initialized bot runner objects.
    /// </summary>
    public sealed class Factory
    {
        private Dictionary<JobEnum, Bot> map = new Dictionary<JobEnum, Bot>();

        public void Add(JobEnum jobId, Bot bot)
        {
            map.Add(jobId, bot);
        }

        public Bot Get(int id)
        {
            return map[(JobEnum)id];
        }
    }
}
