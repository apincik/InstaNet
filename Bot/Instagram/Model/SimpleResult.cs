using System;
using Newtonsoft.Json;

namespace Bot.Instagram.Model
{
    [Serializable]
    public class SimpleResult
    {
        [JsonProperty(PropertyName = "result")]
        public string Result { get; set; }

        public bool IsSuccess()
        {
            return Result == "success";
        }

        public bool IsError()
        {
            return Result == "error";
        }
    }   
}
