using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bot.Instagram.Model
{
    [Serializable]
    public class InstaLog
    {
        public InstaLog()
        {
            Data = new List<Log>();
        }

        [JsonProperty(PropertyName = "data")]
        public List<Log> Data { get; set; }
    }
}
