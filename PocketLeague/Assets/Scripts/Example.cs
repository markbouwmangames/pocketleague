using UnityEngine;
using RLSApi;

public class Example : MonoBehaviour {
	[SerializeField]
	private string value;

	void Awake() {
		var player = RLSClient.FromPlayerData(value);
		Debug.Log(player.displayName);

		/*RLSClient.GetPlayer(RLSApi.Data.RlsPlatform.Ps4, "Mefoz", (data) => {
			Debug.Log(data.displayName);
			Debug.Log(data.rankedSeasons);
		}, null);*/
	}
}
