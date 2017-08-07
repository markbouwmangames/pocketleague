using RLSApi.Data;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerReferenceData {
	[JsonProperty("Platform", Required = Required.Always)]
	public RlsPlatform Platform { get; private set; }
	[JsonProperty("UniqueId", Required = Required.Always)]
	public string UniqueId { get; private set; }
	[JsonProperty("DisplayName", Required = Required.Always)]
	public string DisplayName { get; private set; }
	[JsonIgnore]
	public Texture2D Avatar { get; private set; }

	public PlayerReferenceData(RlsPlatform platform, string uniqueId, string displayName) {
		Platform = platform;
		UniqueId = uniqueId;
		DisplayName = displayName;
	}

	public void SetAvatar(Texture2D avatar) {
		Avatar = avatar;
	}

	public override bool Equals(object obj) {
		if (obj is PlayerReferenceData == false) return false;
		var other = (PlayerReferenceData)obj;
		return this.Platform == other.Platform && this.UniqueId == other.UniqueId;
	}

	public override int GetHashCode() {
		return base.GetHashCode();
	}
}
