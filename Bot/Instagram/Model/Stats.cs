using Newtonsoft.Json;

namespace Bot.Instagram.Model
{
    /**
     * Profile stats 
     */
    public class Stats
    {
        public Stats()
        {
            PostsCount = 0;
            FollowingCount = 0;
            FollowedCount = 0;
        }

        public string Url { get; set; }
        public float PostsCount { get; set; }
        //Number i follow.
        public float FollowingCount { get; set; }
        //Number people who follow me.}  
        public float FollowedCount { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(
                new
                {
                    posts = PostsCount,
                    following = FollowingCount,
                    followed = FollowedCount
                }
            );
        }
    }
}
