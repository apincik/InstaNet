using Bot.Instagram.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Bots
{
    /// <summary>
    /// Bot for likes posts of users from explore feed.
    /// </summary>
    public class Like : Bot
    {
        public override async Task Run(Job job)
        {
            await FollowLike(job, _behaviour.LikesAtOnce, _behaviour.LikesPerTag, _behaviour.SplitLikesPerAllTags, _manager.LikeUserPostFromFeedByTag);
        }
    }
}
