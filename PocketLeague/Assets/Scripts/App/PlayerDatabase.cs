using RLSApi.Net.Models;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class PlayerDatabase : MonoBehaviour {
	private class Data {
		public PlayerReferenceData PlayerReferenceData;
		public Player Player;
	}

	private List<Data> _trackedPlayers = new List<Data>();
	private List<Data> _storedPlayers = new List<Data>();
	private readonly string dataLocation = "::playerDatabase";

	void Awake() {
		var data = PlayerPrefs.GetString(dataLocation, "");
		if (string.IsNullOrEmpty(data) == false) {
			_trackedPlayers = JsonConvert.DeserializeObject<List<Data>>(data);
		}
	}

	#region Session Stored Players
	public void StoreTempPlayer(PlayerReferenceData playerReferenceData, Player player) {
		_storedPlayers.Add(new Data() {
			PlayerReferenceData = playerReferenceData,
			Player = player
		});
	}

	public Player GetStoredPlayer(PlayerReferenceData playerReferenceData) {
		for (var i = 0; i < _storedPlayers.Count; i++) {
			var saveData = _storedPlayers[i];
			if (saveData.PlayerReferenceData.Equals(playerReferenceData)) {
				return _storedPlayers[i].Player;
			}
		}
		return null;
	}

	public void UpdateAvatar(PlayerReferenceData playerReferenceData, Texture2D avatar) {
		for (var i = 0; i < _storedPlayers.Count; i++) {
			var saveData = _storedPlayers[i];
			if (saveData.PlayerReferenceData.Equals(playerReferenceData)) {
				saveData.PlayerReferenceData.SetAvatar(avatar);
			}
		}
	}

	public Texture2D GetAvatar(PlayerReferenceData playerReferenceData) {
		for (var i = 0; i < _storedPlayers.Count; i++) {
			var saveData = _storedPlayers[i];
			if (saveData.PlayerReferenceData.Equals(playerReferenceData)) {
				return saveData.PlayerReferenceData.Avatar;
			}
		}

		return null;
	}
	#endregion

	#region Tracked Players
	public void TrackPlayer(PlayerReferenceData playerReferenceData, Player player) {
		//if database contains player
		for (var i = 0; i < _trackedPlayers.Count; i++) {
			var saveData = _trackedPlayers[i];
			if(saveData.PlayerReferenceData.Equals(playerReferenceData)) {
				_trackedPlayers[i].Player = player;
				StoreData();
				return;
			}
		}

		//else add new player
		_trackedPlayers.Add(new Data() {
			PlayerReferenceData = playerReferenceData,
			Player = player
		});
		StoreData();
	}

	public void RemovePlayer(PlayerReferenceData playerReferenceData) {
		for (var i = 0; i < _trackedPlayers.Count; i++) {
			var saveData = _trackedPlayers[i];
			if (saveData.PlayerReferenceData.Equals(playerReferenceData)) {
				_trackedPlayers.RemoveAt(i);
				return;
			}
		}

		StoreData();
	}

	public PlayerReferenceData[] GetTrackedPlayers() {
		var result = new List<PlayerReferenceData>();
		foreach (var saveData in _trackedPlayers) {
			result.Add(saveData.PlayerReferenceData);
		}
		return result.ToArray();
	}

	public Player GetTrackedPlayerData(PlayerReferenceData playerReferenceData) {
		foreach(var saveData in _trackedPlayers) {
			if(saveData.PlayerReferenceData.Equals(playerReferenceData)) {
				return saveData.Player;
			}
		}
		return null;
	}

	private void StoreData() {
		var data = JsonConvert.SerializeObject(_trackedPlayers);
		PlayerPrefs.SetString(dataLocation, data);
	}
	#endregion
}
