using System;
using Newtonsoft.Json;

namespace Bot.Instagram.Model
{

    [Serializable]
    public class Proxy
    {
        [JsonProperty(PropertyName = "ipaddress")]
        public string IpAddress { get; set; }
        [JsonProperty(PropertyName = "port")]
        public string Port { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        public bool isLocal()
        {
            return IpAddress == "127.0.0.1";
        }
    }
}
