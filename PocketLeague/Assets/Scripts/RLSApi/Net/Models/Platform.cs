﻿using Newtonsoft.Json;

namespace RLSApi.Net.Models
{
    public class Platform
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
    }
}
