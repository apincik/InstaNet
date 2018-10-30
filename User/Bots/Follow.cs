using Bot.Instagram.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Bots
{
    /// <summary>
    /// Bot for following users from explore feed.
    /// </summary>
    public class Follow : Bot
    {
        public override async Task Run(Job job)
        {
            await FollowLike(job, _behaviour.FollowsAtOnce, _behaviour.FollowsPerTag, _behaviour.SplitFollowsPerAllTags, _manager.FollowFeedUsersByTag);
        }
    }
}
