using UnityEngine;
using RLSApi;

public class Example : MonoBehaviour {
	void Awake() {
		RLSClient.GetPlayer(RLSApi.Data.RlsPlatform.Ps4, "Mefoz", (data) => {
			Debug.Log(data.RankedSeasons[RLSApi.Data.RlsSeason.Five][RLSApi.Data.RlsPlaylistRanked.Doubles].RankPoints);
		}, null);
	}
}
