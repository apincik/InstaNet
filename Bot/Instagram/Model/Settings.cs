using System;
using Newtonsoft.Json;

namespace Bot.Instagram.Model
{

    [Serializable]
    public class Settings
    {
        [JsonProperty(PropertyName = "likesAtOnce")]
        public int LikesAtOnce { get; set; }
        [JsonProperty(PropertyName = "likesPerTag")]
        public int LikesPerTag { get; set; }
        [JsonProperty(PropertyName = "splitLikesPerAllTags")]
        public int SplitLikesPerAllTags { get; set; }
        [JsonProperty(PropertyName = "followsAtOnce")]
        public int FollowsAtOnce { get; set; }
        [JsonProperty(PropertyName = "followsPerTag")]
        public int FollowsPerTag { get; set; }
        [JsonProperty(PropertyName = "splitFollowsPerAllTags")]
        public int SplitFollowsPerAllTags { get; set; }
        [JsonProperty(PropertyName = "randomActionsOrder")]
        public int RandomActionsOrder { get; set; }

        [JsonProperty(PropertyName = "minFollowersToLikePost")]
        public int MinFollowersToLikePost { get; set; }
        [JsonProperty(PropertyName = "maxFollowersToLikePost")]
        public int MaxFollowersToLikePost { get; set; }
        [JsonProperty(PropertyName = "minFollowingToLikePost")]
        public int MinFollowingToLikePost { get; set; }
        [JsonProperty(PropertyName = "maxFollowingToLikePost")]
        public int MaxFollowingToLikePost { get; set; }

        [JsonProperty(PropertyName = "minFollowersToFollowUser")]
        public int MinFollowersToFollowUser { get; set; }
        [JsonProperty(PropertyName = "maxFollowersToFollowUser")]
        public int MaxFollowersToFollowUser { get; set; }
        [JsonProperty(PropertyName = "minFollowingToFollowUser")]
        public int MinFollowingToFollowUser { get; set; }
        [JsonProperty(PropertyName = "maxFollowingToFollowUser")]
        public int MaxFollowingToFollowUser { get; set; }
        [JsonProperty(PropertyName = "minPostsToFollowUser")]
        public int MinPostsToFollowUser { get; set; }
        [JsonProperty(PropertyName = "maxPostsToFollowUser")]
        public int MaxPostsToFollowUser { get; set; }

        [JsonProperty(PropertyName = "minPostLikesToLikePost")]
        public int MinPostLikesToLikePost { get; set; }
        [JsonProperty(PropertyName = "maxPostLikesToLikePost")]
        public int MaxPostLikesToLikePost { get; set; }

        [JsonProperty(PropertyName = "unfollowAtOnce")]
        public int UnfollowAtOnce { get; set; }
        [JsonProperty(PropertyName = "unfollowIsRandom")]
        public int UnfollowIsRandom { get; set; }

    }
}
