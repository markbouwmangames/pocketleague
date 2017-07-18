﻿using Newtonsoft.Json;

namespace RLSApi.Net.Models
{
    public class Error
    {
        [JsonProperty("code", Required = Required.Always)]
        public int Code { get; set; }

        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; set; }
    }
}
