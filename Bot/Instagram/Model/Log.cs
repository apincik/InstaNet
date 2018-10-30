using System;
using Newtonsoft.Json;

namespace Bot.Instagram.Model
{
    [Serializable]
    public class Log
    {
        [JsonProperty(PropertyName = "scheduleId")]
        public int ScheduleId { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "link")]
        public string Link { get; set; }

        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }
    }
}
