using System;
using Newtonsoft.Json;

namespace Bot.Instagram.Model
{
    [Serializable]
    public class Image
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        public bool IsLocal { get; set; }
        public string LocalPath { get; set; }
    }
}
