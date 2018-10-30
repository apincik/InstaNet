using System;
using Newtonsoft.Json;

namespace Bot.Instagram.Model
{

    [Serializable]
    public class Instagram
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}
