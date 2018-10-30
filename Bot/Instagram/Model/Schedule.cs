using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bot.Instagram.Model
{
    [Serializable]
    public class Schedule
    {
        public Schedule()
        {
            Jobs = new List<Job>();
        }

        [JsonProperty(PropertyName = "data")]
        public List<Job> Jobs { get; set; }
    }
}
