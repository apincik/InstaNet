using Bot.Exception;
using Bot.Instagram.Model;
using System.Threading.Tasks;

namespace User.Bots
{
    /// <summary>
    /// Bot for unfollow users from my followed list.
    /// </summary>
    /// <seealso cref="Account"/>
    public class Unfollow : Bot
    {
        public override async Task Run(Job job)
        {
            int totalUnFollows = 0;
            int toDoUnFollows = job.Limit - job.LimitDone;
            int unfollowsCount = _behaviour.UnfollowAtOnce;
            if (unfollowsCount > toDoUnFollows)
            {
                unfollowsCount = toDoUnFollows;
            }

            totalUnFollows = await _manager.UnfollowFollowedUsers(unfollowsCount, _behaviour.UnfollowIsRandom);
            await _client.UpdateJobProcess(job.Id, totalUnFollows);

            if (job.LimitDone + totalUnFollows >= job.Limit)
            {
                await _client.SetJobProcessed(job.Id);
            }
        }
    }
}
