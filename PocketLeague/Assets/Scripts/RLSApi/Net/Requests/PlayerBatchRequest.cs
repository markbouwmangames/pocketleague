using Newtonsoft.Json;
using RLSApi.Data;

namespace RLSApi.Net.Requests {
	public class PlayerBatchRequest {
		public PlayerBatchRequest(RlsPlatform platform, string uniqueId) {
			Platform = platform;
			UniqueId = uniqueId;
		}

		[JsonProperty("platformId", Required = Required.Always)]
		public RlsPlatform Platform { get; private set; }

		[JsonProperty("uniqueId", Required = Required.Always)]
		public string UniqueId { get; private set; }
	}
}
