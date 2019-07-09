﻿using Newtonsoft.Json;

namespace Discord
{
    public class Attachment
    {
        [JsonProperty("id")]
        public ulong Id { get; private set; }


        [JsonProperty("url")]
        public string Url { get; private set; }


        [JsonProperty("proxy_url")]
        public string ProxyUrl { get; private set; }


        [JsonProperty("size")]
        public uint FileSize { get; private set; }


        public override string ToString()
        {
            return Url;
        }
    }
}