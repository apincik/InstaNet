using System;
using Bot.Instagram.Model;

namespace Bot.Instagram
{
    /// <summary>
    /// Bot behaviour settings.
    /// </summary>
    public class Behaviour
    {
        /*
         * Likes per one job.
         */
        public int LikesAtOnce { get; set; }
        public int LikesPerTag { get; set; }
        public bool SplitLikesPerAllTags { get; set; }

        /**
         * Follows per one job.
         */
        public int FollowsAtOnce { get; set; }
        public int FollowsPerTag { get; set; }
        public bool SplitFollowsPerAllTags { get; set; }

        /**
         * Unfollow per action 
         */
        public int UnfollowAtOnce { get; set; }
        public bool UnfollowIsRandom { get; set; }

        /**
         * Stats to like any user post on profile
         */
        public int MinFollowersToLikeUserPost { get; set; }
        public int MaxFollowersToLikeUserPost { get; set; }
        public int MinFollowingToLikeUserPost { get; set; }
        public int MaxFollowingToLikeUserPost { get; set; }

        public int MinFollowersToFollowUser { get; set; }
        public int MaxFollowersToFollowUser { get; set; }
        public int MinFollowingToFollowUser { get; set; }
        public int MaxFollowingToFollowUser { get; set; }
        public int MinPostsToFollowUser { get; set; }
        public int MaxPostsToFollowUser { get; set; }

        public int MinPostLikesToLikePost { get; set; }

        public int MaxPostLikesToLikePost { get; set; }

        /**
         * Random content select from feed.
         */
        //public bool RandomActionsOrder { get; set; }
        //public int RandomActionsMax { get; set; }

        /**
         * Timing. 
         */
        //public int WaitBetweenActionsSeconds { get; set; }
        //public bool WaitBetweenActionsRandom { get; set; }
        //public int WaitBetweenActionsRandomMax { get; set; }

        /**
         * How many user posts should be liked once profile opened
         */
        //public int PostsLikesCountPerUser { get; set; }

        public Behaviour()
        {
            Random rand = new Random();

            LikesAtOnce = 30;
            LikesPerTag = 5;
            SplitLikesPerAllTags = true;

            FollowsAtOnce = 5;
            FollowsPerTag = 3;
            SplitFollowsPerAllTags = true;

            UnfollowAtOnce = 5;
            UnfollowIsRandom = true;

            MinFollowersToLikeUserPost = 0;
            MinFollowingToLikeUserPost = 0;
            MaxFollowersToLikeUserPost = rand.Next(1000, 2000);
            MaxFollowingToLikeUserPost = rand.Next(800, 2000);

            MinFollowersToFollowUser = 0;
            MinFollowingToFollowUser = 0;
            MaxFollowersToFollowUser = rand.Next(1000, 2000);
            MaxFollowingToFollowUser = rand.Next(800, 2000);
            MinPostsToFollowUser = 0;
            MaxPostsToFollowUser = 0;

            MinPostLikesToLikePost = 0;
            MaxPostLikesToLikePost = rand.Next(25, 50);
            if (UnfollowIsRandom)
            {
                UnfollowAtOnce = rand.Next(1, UnfollowAtOnce);
            }

            //RandomActionsOrder = true;
            //RandomActionsMax = 10;
            //WaitBetweenActionsSeconds = 10;
            //WaitBetweenActionsRandom = true;
            //WaitBetweenActionsRandomMax = 30;
            //PostsLikesCountPerUser = 1;
        }

        public void RandomRefresh()
        {
            Random rand = new Random();

            LikesAtOnce = rand.Next(25, 35);
            LikesPerTag = rand.Next(4, 7);
            FollowsAtOnce = rand.Next(3, 7);
            FollowsPerTag = rand.Next(1, 4);
            UnfollowAtOnce = rand.Next(1, 10);
        }

        public void SetValues(Settings settings)
        {
            Random rand = new Random();

            int minLikesAtOnce = settings.LikesAtOnce - (int)Math.Ceiling(((double)(settings.LikesAtOnce / 100) * 20));    //Min - 20%
            int maxLikesAtOnce = settings.LikesAtOnce + (int)Math.Ceiling(((double)(settings.LikesAtOnce / 100) * 20));   //Max + 20%
            int randLikesAtOnce = rand.Next(minLikesAtOnce, maxLikesAtOnce);

            int minFollowsAtOnce = settings.FollowsAtOnce - (int)Math.Ceiling(((double)(settings.FollowsAtOnce / 100) * 20));    //Min - 20%
            int maxFollowsAtOnce = settings.FollowsAtOnce + (int)Math.Ceiling(((double)(settings.FollowsAtOnce / 100) * 20));   //Max + 20%
            int randFollowsAtOnce = rand.Next(minFollowsAtOnce, maxFollowsAtOnce);

            //@todo uncomment for randomized values
            //LikesAtOnce = randLikesAtOnce;
            //LikesPerTag = settings.LikesPerTag;
            SplitLikesPerAllTags = settings.SplitLikesPerAllTags == 1;
            //FollowsAtOnce = randFollowsAtOnce;
            //FollowsPerTag = settings.FollowsPerTag;
            SplitFollowsPerAllTags = settings.SplitFollowsPerAllTags == 1;
            //RandomActionsOrder = settings.RandomActionsOrder == 1;

            MinFollowersToLikeUserPost = settings.MinFollowersToLikePost;
            MaxFollowersToLikeUserPost = settings.MaxFollowersToLikePost;
            MinFollowingToLikeUserPost = settings.MinFollowingToLikePost;
            MaxFollowingToLikeUserPost = settings.MaxFollowingToLikePost;

            MinFollowersToFollowUser = settings.MinFollowersToFollowUser;
            MaxFollowersToFollowUser = settings.MaxFollowersToFollowUser;
            MinFollowingToFollowUser = settings.MinFollowingToFollowUser;
            MaxFollowingToFollowUser = settings.MaxFollowingToFollowUser;
            MinPostsToFollowUser = settings.MinPostsToFollowUser;
            MaxPostsToFollowUser = settings.MaxPostsToFollowUser;

            MinPostLikesToLikePost = settings.MinPostLikesToLikePost;
            MaxPostLikesToLikePost = settings.MaxPostLikesToLikePost;

            UnfollowAtOnce = settings.UnfollowAtOnce;
            UnfollowIsRandom = settings.UnfollowIsRandom == 1;
        }
    }
}
