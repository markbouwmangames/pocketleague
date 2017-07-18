using UnityEngine;
using RLSApi;
using RLSApi.Net.Requests;
using RLSApi.Data;

public class Example : MonoBehaviour {
	void Awake() {
		RLSClient.GetPlayers(new[] {
				new PlayerBatchRequest(RlsPlatform.Steam, "76561198033338223"),
				new PlayerBatchRequest(RlsPlatform.Ps4, "Wizwonk"),
				new PlayerBatchRequest(RlsPlatform.Xbox, "Loubleezy")
	}, (data) => {
		Debug.Log(data[0].DisplayName);
		Debug.Log(data[1].DisplayName);
	}, null);
	}
}
