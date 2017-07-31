using RLSApi.Data;

public class PlayerReferenceData {
	public RlsPlatform Platform;
	public string DisplayName;

	public override bool Equals(object obj) {
		if (obj is PlayerReferenceData == false) return false;
		var other = (PlayerReferenceData)obj;
		return this.Platform == other.Platform && this.DisplayName == other.DisplayName;
	}

	public override int GetHashCode() {
		return base.GetHashCode();
	}
}
