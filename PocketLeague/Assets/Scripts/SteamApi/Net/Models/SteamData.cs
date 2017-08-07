using Newtonsoft.Json;

namespace SteamApi.Net.Models {
	public class SteamData {
		[JsonProperty("success", Required = Required.Always)]
		public int Succes;
		[JsonProperty("steamid", Required = Required.Default)]
		public string Id;
		[JsonProperty("message ", Required = Required.Default)]
		public string Message;
	}

	public class SteamResponse {
		[JsonProperty("response", Required = Required.Always)]
		public SteamData SteamData;
	}
}