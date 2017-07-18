using UnityEngine;
using RLSApi;
using RLSApi.Models;

public class Example : MonoBehaviour {
	void Awake() {
		RLSClient.GetTiers(null, null);
	}

	private void OnGetTiers(Tier[] tiers) {
		Debug.Log("Got: " + tiers.Length + " tiers.");
		foreach(var t in tiers) {
			Debug.Log(t.tierName);
		}
	}

	private void OnGetTiersFail(Error error) {
		Debug.Log("Got error with code: " + error.Code);
		Debug.Log(error.Message);
	}
}
