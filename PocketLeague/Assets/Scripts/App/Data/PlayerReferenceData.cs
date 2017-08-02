using RLSApi.Data;
using UnityEngine;

public class PlayerReferenceData {
	public RlsPlatform Platform { get; private set; }
	public string UniqueId { get; private set; }
	public string DisplayName { get; private set; }
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
		return this.Platform == other.Platform && this.DisplayName == other.DisplayName;
	}

	public override int GetHashCode() {
		return base.GetHashCode();
	}
}
