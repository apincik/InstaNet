using System;
using Newtonsoft.Json;

namespace Bot.Instagram.Model
{
    [Serializable]
    public class Action
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }


    }
}
