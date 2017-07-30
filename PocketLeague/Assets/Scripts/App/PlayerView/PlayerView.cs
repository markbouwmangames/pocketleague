using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RLSApi.Net.Models;
using RLSApi;
using System;

public class PlayerView : BaseUpdateView {
	protected override void UpdateView(Action OnComplete = null) {
		Loader.OnLoadStart();
	}

	public void Search(PlayerReferenceData playerReference) {
		RLSClient.GetPlayer(playerReference.Platform, playerReference.DisplayName, (player) => {
			//success
			SetPlayer(player);
		}, (error) => {
			//error
		});
	}

	public void SetPlayer(Player player) {
		var children = GetComponentsInChildren<PlayerViewChild>();

		foreach(var child in children) {
			child.Set(player);
		}
		Loader.OnLoadEnd();
	}
}
