using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RLSApi.Net.Models;
using RLSApi;
using System;
using RLSApi.Data;
using RLSApi.Util;

public class PlayerView : BaseUpdateView {
    private PlayerReferenceData _currentPlayer;

	protected override void UpdateView(Action onComplete = null) {
        if (onComplete == null) Loader.OnLoadStart();

		var database = FindObjectOfType<PlayerDatabase>();
		var player = database.GetStoredPlayer(_currentPlayer);

		if(player != null) {
			var difference = TimeUtil.Difference(DateTimeOffset.UtcNow, player.NextUpdateAt);
			if (difference > 0) {
				SetPlayerData(player);
				if (onComplete != null) onComplete.Invoke();
				else Loader.OnLoadEnd();
				return;
			}
		}

        GetPlayerData(_currentPlayer, (data) => {
			//success
			SetPlayerData(data);
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
        RLSClient.GetPlayer(playerReference.Platform, playerReference.UniqueId, onSuccess, onFail);
    }

	public void SetPlayerData(Player player) {
		var database = FindObjectOfType<PlayerDatabase>();
		database.StoreTempPlayer(_currentPlayer, player);

		if (database.GetTrackedPlayerData(_currentPlayer) != null) {
			database.TrackPlayer(_currentPlayer, player);
		}

		var children = GetComponentsInChildren<PlayerViewChild>();
		foreach(var child in children) {
			child.Set(player);
		}
	}
}
