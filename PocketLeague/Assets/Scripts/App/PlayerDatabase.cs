using RLSApi.Net.Models;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class PlayerDatabase : MonoBehaviour {
	private class Data {
		public PlayerReferenceData PlayerReferenceData;
		public Player Player;
	}

	private List<Data> _storedPlayers = new List<Data>();
	private readonly string dataLocation = "::playerDatabase_4";

	void Awake() {
		var data = PlayerPrefs.GetString(dataLocation, "");
		if (string.IsNullOrEmpty(data) == false) {
			_storedPlayers = JsonConvert.DeserializeObject<List<Data>>(data);
		}
	}

	public void AddPlayer(PlayerReferenceData playerReferenceData, Player player) {
		_storedPlayers.Add(new Data() {
			PlayerReferenceData = playerReferenceData,
			Player = player
		});

		StoreData();
	}

	public void RemovePlayer(PlayerReferenceData playerReferenceData) {
		for (var i = 0; i < _storedPlayers.Count; i++) {
			var saveData = _storedPlayers[i];
			if (saveData.PlayerReferenceData.Equals(playerReferenceData)) {
				_storedPlayers.RemoveAt(i);
				return;
			}
		}

		StoreData();
	}

	public void UpdatePlayer(PlayerReferenceData playerReferenceData, Player player) {
		for (var i = 0; i < _storedPlayers.Count; i++) {
			var saveData = _storedPlayers[i];
			if(saveData.PlayerReferenceData.Equals(playerReferenceData)) {
				_storedPlayers[i] = new Data() {
					PlayerReferenceData = playerReferenceData,
					Player = player
				};
				return;
			}
		}
		StoreData();
	}

	public bool ContainsPlayer(PlayerReferenceData playerReferenceData) {
		for (var i = 0; i < _storedPlayers.Count; i++) {
			var saveData = _storedPlayers[i];
			if (saveData.PlayerReferenceData.Equals(playerReferenceData)) {
				return true;
			}
		}
		return false;
	}


	public PlayerReferenceData[] GetReferencedPlayers() {
		var result = new List<PlayerReferenceData>();
		foreach (var saveData in _storedPlayers) {
			result.Add(saveData.PlayerReferenceData);
		}
		return result.ToArray();
	}

	public Player GetLocalPlayerData(PlayerReferenceData playerReferenceData) {
		foreach(var saveData in _storedPlayers) {
			if(saveData.PlayerReferenceData.Equals(playerReferenceData)) {
				return saveData.Player;
			}
		}
		return null;
	}

	private void StoreData() {
		var data = JsonConvert.SerializeObject(_storedPlayers);
		PlayerPrefs.SetString(dataLocation, data);
	}
}
