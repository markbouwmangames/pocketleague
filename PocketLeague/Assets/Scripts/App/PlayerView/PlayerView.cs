using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RLSApi.Net.Models;
using RLSApi;

public class PlayerView : MonoBehaviour {
	void Awake() {
		RLSClient.GetPlayer(RLSApi.Data.RlsPlatform.Ps4, "Mefoz", (player) => {
			SetPlayer(player);
		}, null);
	}

	public void SetPlayer(Player player) {
		var children = GetComponentsInChildren<PlayerViewChild>();

		foreach(var child in children) {
			child.Set(player);
		}
	}
}
