using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RLSApi.Net.Models;
using RLSApi;
using System;

public class PlayerView : BaseUpdateView {
    private PlayerReferenceData _currentPlayer;

    protected override void UpdateView(Action onComplete = null) {
        if (onComplete == null) Loader.OnLoadStart();

        GetPlayerData(_currentPlayer, (player) => {
            //success
            SetPlayer(player);
            if (onComplete != null) onComplete.Invoke();
            else Loader.OnLoadEnd();
        }, (error) => {
            //fail
            Debug.LogWarning("TODO: ERROR HANDLING");
        });
	}

    public void SetPlayer(PlayerReferenceData playerReference) {
        _currentPlayer = playerReference;
	}

    private void GetPlayerData(PlayerReferenceData playerReference, Action<Player> onSuccess, Action<Error> onFail) {
        RLSClient.GetPlayer(playerReference.Platform, playerReference.DisplayName, onSuccess, onFail);
    }

	public void SetPlayer(Player player) {
		var database = FindObjectOfType<PlayerDatabase>();
		if (database.ContainsPlayer(_currentPlayer)) {
			database.UpdatePlayer(_currentPlayer, player);
		}


		var children = GetComponentsInChildren<PlayerViewChild>();
		foreach(var child in children) {
			child.Set(player);
		}
	}
}
