using Newtonsoft.Json;

namespace RLSApi.Net.Models
{
    public class Error {
        [JsonProperty("code", Required = Required.Always)]
        public long StatusCode { get; set; }
        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; set; }
    }
}
