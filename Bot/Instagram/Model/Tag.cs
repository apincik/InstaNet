using System;
using Newtonsoft.Json;

namespace Bot.Instagram.Model
{
    [Serializable]
    public class Tag
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public int Priority { get; set; }

        //Limit of actions per tag.
        public int Limit = 0;
    }
}
