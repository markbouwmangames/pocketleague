using Newtonsoft.Json;

namespace RLSApi.Net.Models
{
    public class PlayerStats
    {
        [JsonProperty("wins", Required = Required.Always)]
        public int Wins { get; set; }

        [JsonProperty("goals", Required = Required.Always)]
        public int Goals { get; set; }

        [JsonProperty("mvps", Required = Required.Always)]
        public int Mvps { get; set; }

        [JsonProperty("saves", Required = Required.Always)]
        public int Saves { get; set; }

        [JsonProperty("shots", Required = Required.Always)]
        public int Shots { get; set; }

        [JsonProperty("assists", Required = Required.Always)]
        public int Assists { get; set; }

		[JsonIgnore]
		public float ShotAccuracy {
			get {
				return UnityEngine.Mathf.Round(((float)(Goals) / (float)(Shots)) * 10000f) / 100f;
			}
		}

		[JsonIgnore]
		public float MvpPercentage {
			get {
				return UnityEngine.Mathf.Round(((float)(Mvps) / (float)(Wins)) * 10000f) / 100f;
			}
		}
	}
}
