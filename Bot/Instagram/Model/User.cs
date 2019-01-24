using System;
using Newtonsoft.Json;

namespace Bot.Instagram.Model
{

    [Serializable]
    public class User
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url{ get; set; }
        [JsonProperty(PropertyName = "profileImage")]
        public string ProfileImage { get; set; }
    }
}
