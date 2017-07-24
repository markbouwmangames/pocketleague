using Newtonsoft.Json;
using System;

namespace Twitch.Net.Models {
    public class StreamData {
        [JsonProperty("_total", Required = Required.Always)]
        public int Total;
        [JsonProperty("streams", Required = Required.Always)]
        public Stream[] Streams;
    }

    public class Stream {
        [JsonProperty("_id", Required = Required.Always)]
        public string Id { get; set; }
        [JsonProperty("viewers", Required = Required.Always)]
        public int Viewers { get; set; }
        [JsonProperty("created_at", Required = Required.Always)]
        public DateTimeOffset CreatedAt { get; set; }
        [JsonProperty("preview", Required = Required.Always)]
        public ChannelPreview Preview { get; set; }
    }

    public class ChannelPreview {
        [JsonProperty("small", Required = Required.Always)]
        public string Small { get; set; }
        [JsonProperty("medium", Required = Required.Always)]
        public string Medium { get; set; }
        [JsonProperty("large", Required = Required.Always)]
        public string Large { get; set; }
        [JsonProperty("template", Required = Required.Always)]
        public string Template { get; set; }
    }

    public class Channel {
        [JsonProperty("status", Required = Required.Always)]
        public string Status { get; set; }
        [JsonProperty("display_name", Required = Required.Always)]
        public string DisplayName { get; set; }
        [JsonProperty("created_at", Required = Required.Always)]
        public DateTimeOffset CreatedAt { get; set; }
        [JsonProperty("url", Required = Required.Always)]
        public string URL{ get; set; }
    }
}

