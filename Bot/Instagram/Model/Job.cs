using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bot.Instagram.Model
{
    [Serializable]
    public class Job
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "instagram")]
        public Instagram Instagram { get; set; }

        [JsonProperty(PropertyName = "targetTag")]
        public string TargetTag { get; set; }

        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public List<Tag> Tags { get; set; }

        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }

        [JsonProperty(PropertyName = "limit")]
        public int Limit { get; set; }

        [JsonProperty(PropertyName = "limitDone")]
        public int LimitDone { get; set; }

        [JsonProperty(PropertyName = "runCount")]
        public int RunCount { get; set; }

        [JsonProperty(PropertyName = "proxy")]
        public Proxy Proxy { get; set; }

        [JsonProperty(PropertyName = "settings")]
        public Settings Settings { get; set; }
    }
}
